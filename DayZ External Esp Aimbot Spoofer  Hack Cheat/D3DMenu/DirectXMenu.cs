using D3DMenu.Attributes;
using D3DMenu.MenuTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3DMenu
{
    public class DirectXMenu
    {
        #region Globals

        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x0101;

        private const string settingsFileName = "settings.bin";

        #endregion

        #region Properties

        public List<SubMenu> SubMenus { get; set; }

        public bool IsVisible { get; set; }

        public int SelectedSubMenuCount { get; set; }
        public int SelectedSubMenuIndex { get; set; }
        public int SelectedSubMenuItem { get; set; } // The selected item inside a submenu. Eg. BoolSwitch
        public int MarkedSubMenuIndex { get; set; } // Selected index in the next drawing loop

        #endregion

        #region Constructor

        public DirectXMenu()
        {
            SelectedSubMenuIndex = -1;
            SelectedSubMenuCount = 0;
            SubMenus = new List<SubMenu>();
        }

        #endregion

        #region Methods

        public void Save()
        {
            // Contains all values. Key = name of the menu item, value = FieldValue Attribute
            Dictionary<string, object> values = new Dictionary<string, object>();

            foreach (SubMenu subMenu in SubMenus)
            {
                foreach (MenuItemBase menuItem in subMenu.Items)
                {
                    if (!menuItem.Save)
                        continue;

                    System.Attribute[] attrs = System.Attribute.GetCustomAttributes(menuItem.GetType());  // Reflection.

                    // Displaying output.
                    foreach (System.Attribute attr in attrs)
                    {
                        if (attr is ValueField)
                        {
                            ValueField valueField = attr as ValueField;
                            object value = menuItem.GetType().GetProperty(valueField.Name).GetValue(menuItem, null);

                            values.Add(menuItem.Name, value);
                        }
                    }
                }
            }

            using (Stream stream = File.Open(settingsFileName, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bformatter.Serialize(stream, values);
            }
        }

        public void Load()
        {
            if (!File.Exists(settingsFileName))
                return;

            Dictionary<string, object> values;
            using (Stream stream = File.Open(settingsFileName, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                values = (Dictionary<string, object>)bformatter.Deserialize(stream);
            }

            foreach (KeyValuePair<string, object> value in values)
            {
                foreach (SubMenu subMenu in SubMenus)
                {
                    MenuItemBase menuItem = subMenu.Items.FirstOrDefault(i => i.Name == value.Key);
                    if(menuItem != null)
                    {
                        System.Attribute[] attrs = System.Attribute.GetCustomAttributes(menuItem.GetType());

                        foreach (System.Attribute attr in attrs)
                        {
                            if (attr is ValueField)
                            {
                                ValueField valueField = attr as ValueField;
                                menuItem.GetType().GetField(valueField.Name).SetValue(menuItem, value.Value);
                            }
                        }
                    }
                }
            }
        }

        public bool KeyPress(int code, IntPtr wParam, IntPtr lParam)
        {
            bool ValidKeyDown = false;
            if (code == Keys.Up.GetHashCode()
                || code == Keys.Down.GetHashCode() || code == Keys.Right.GetHashCode()
                || code == Keys.Left.GetHashCode() || code == Keys.Insert.GetHashCode()
                || code == Keys.NumPad2.GetHashCode() || code == Keys.NumPad8.GetHashCode()
                || code == Keys.NumPad5.GetHashCode() || code == Keys.NumPad9.GetHashCode()
                || code == Keys.NumPad3.GetHashCode())
                ValidKeyDown = true;

            if (code >= 0 && wParam == (IntPtr)WM_KEYDOWN && ValidKeyDown)
            {
                //Hide/Show Menu
                if (code == Keys.Insert.GetHashCode())
                    IsVisible = !IsVisible;

                if (!IsVisible)
                    return false;

                int menuItemCount = 0;
                if (SelectedSubMenuIndex < 0)
                    menuItemCount = SubMenus.Count;
                else
                    menuItemCount = SelectedSubMenuCount;


                //Main menu
                if (SelectedSubMenuIndex < 0)
                {
                    if (code == Keys.Up.GetHashCode() && MarkedSubMenuIndex > 0)
                        MarkedSubMenuIndex--;
                    else if (code == Keys.Up.GetHashCode() && MarkedSubMenuIndex == 0)
                        MarkedSubMenuIndex = menuItemCount - 1;

                    if (code == Keys.Down.GetHashCode() && MarkedSubMenuIndex < menuItemCount - 1)
                        MarkedSubMenuIndex++;
                    else if (code == Keys.Down.GetHashCode() && MarkedSubMenuIndex == menuItemCount - 1)
                        MarkedSubMenuIndex = 0;

                    if (code == Keys.Right.GetHashCode() && SubMenus[MarkedSubMenuIndex].Enabled)
                        SelectedSubMenuIndex = MarkedSubMenuIndex;
                }
                else //Sub Menu
                {
                    if (code == Keys.Up.GetHashCode() && SelectedSubMenuItem > 0)
                        SelectedSubMenuItem--;
                    else if (code == Keys.Up.GetHashCode() && SelectedSubMenuItem == 0)
                        SelectedSubMenuItem = menuItemCount - 1;

                    if (code == Keys.Down.GetHashCode() && SelectedSubMenuItem < menuItemCount - 1)
                        SelectedSubMenuItem++;
                    else if (code == Keys.Down.GetHashCode() && SelectedSubMenuItem == menuItemCount - 1)
                        SelectedSubMenuItem = 0;


                    // Value changes
                    if (code == Keys.Right.GetHashCode())
                    {
                        MenuItemBase menuitem = SubMenus[SelectedSubMenuIndex].Items.Where(sub => sub.DrawMe).ElementAt(SelectedSubMenuItem);

                        if (menuitem.GetType() == typeof(Back))
                        {
                            SelectedSubMenuItem = 0;
                            SelectedSubMenuIndex = -1;
                        }
                        else
                        {
                            menuitem.Next();
                        }
                    }
                    else if (code == Keys.Left.GetHashCode())
                    {
                        MenuItemBase menuitem = SubMenus[SelectedSubMenuIndex].Items.Where(sub => sub.DrawMe).ElementAt(SelectedSubMenuItem);

                        if (menuitem.GetType() == typeof(Back))
                        {
                            SelectedSubMenuItem = 0;
                            SelectedSubMenuIndex = -1;
                        }
                        else
                        {
                            menuitem.Previous();
                        }
                    }
                }
                return true;
            }
            else
                return false;
        }

        #endregion
    }
}
