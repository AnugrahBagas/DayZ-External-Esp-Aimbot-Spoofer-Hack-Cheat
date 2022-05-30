using D3DMenu.MenuTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DMenu
{
    public class SubMenu
    {
        public string Title { get; set; }

        public bool Enabled { get; set; }

        public List<MenuItemBase> Items { get; set; }

        public int Layer { get; set; }

        public SubMenu(string title, int layer = 0, bool enabled = true)
        {
            this.Title = title;
            this.Enabled = enabled;
            this.Layer = layer;
            Items = new List<MenuItemBase>();
        }

        public SubMenu() { }
    }
}
