using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.ScrollMenu
{
    public class D3DScrollMenu
    {
        #region Globals

        // Base
        private DirectXBase d3dBase;
        private TextFormat baseFont;

        // Vars
        private int selectedItemIndex = 0;
        private int selectedCategoryIndex = 0;

        private ScrollMenuCategory selectedCategory;
        private ScrollMenuBaseItem selectedItem;

        // Mouse
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MBUTTONUP = 0x208;

        #endregion

        #region Properties

        public bool IsVisible = false;
        public List<ScrollMenuCategory> Categories { get; set; }


        public float PositionX = 500;
        public float PositionY = 100;

        public float ItemHeight = 20;
        public float CategoryTitleHeight = 20;

        public float MarkerBorderThickness = 2;

        public Color TextColor = Color.White;
        public Color MarkerColor = new Color(200, 200, 200);
        public Color BoxBackground = new Color(30, 30, 30, 200);
        public Color TitleBackground = new Color(10, 10, 10, 200);

        #endregion

        #region Constrcutor

        public D3DScrollMenu(DirectXBase directXBase)
        {
            d3dBase = directXBase;
            baseFont = new TextFormat(d3dBase.FontBase, "Lucida Sans", 13f);
            Categories = new List<ScrollMenuCategory>();
        }

        #endregion

        #region Methods

        public void Draw()
        {
            if (!IsVisible)
                return;

            if (selectedCategory == null && Categories.Count(c => c.IsVisible) > 0)
            {
                selectedCategory = Categories.FirstOrDefault(c => c.IsVisible);
                selectedItem = selectedCategory.Items.FirstOrDefault(i => i.IsVisible);
            }

            if (!selectedItem.IsVisible)
                SelectNextItem();


            // Visibility checks
            DoVisibilityChecks();

            // Background
            float boxWidth = 300;
            float boxHeight = CalculateMenuHeight() + 10;
            float boxX = PositionX - (boxWidth / 2);
            float boxY = PositionY - (boxHeight / 2);
            d3dBase.DrawFilledBox(boxX, boxY, boxWidth, boxHeight, BoxBackground);

            // Title
            float titleHeight = 20;
            d3dBase.DrawFilledBox(boxX, boxY - titleHeight, boxWidth, titleHeight, TitleBackground);
            d3dBase.DrawCenterText(baseFont, "Quick Menu", new RectangleF(boxX, boxY - titleHeight, boxWidth, titleHeight), TextColor);

            // Vars
            float textYOffset = PositionY - (boxHeight / 2);
            float textX = PositionX - (boxWidth / 2);

            foreach (ScrollMenuCategory category in Categories.Where(c => c.IsVisible))
            {
                // Category title
                string categoryTitle = string.Format("--- {0} ---", category.Title);
                d3dBase.DrawCenterText(baseFont, categoryTitle, new RectangleF(textX, textYOffset, boxWidth, ItemHeight), Color.White);
                textYOffset += ItemHeight;

                foreach (ScrollMenuBaseItem item in category.Items.Where(i => i.IsVisible))
                {
                    if (category == selectedCategory && item == selectedItem)
                    {
                        MarkerColor.A = 150;
                        d3dBase.DrawFilledBox(textX, textYOffset, boxWidth, ItemHeight, MarkerColor);
                    }

                    if (item.GetType() == typeof(ScrollMenuBoolItem))
                    {
                        // Bool item
                        ScrollMenuBoolItem boolItem = item as ScrollMenuBoolItem;
                        d3dBase.DrawShadowText(boolItem.Text, baseFont, textX + 3, textYOffset, TextColor);
                        string state = boolItem.State ? "ON" : "OFF";
                        Color stateColor = boolItem.State ? Color.Green : Color.Red;
                        d3dBase.DrawShadowText(state, baseFont, (boxX + boxWidth - 30), textYOffset, stateColor);
                    }
                    else if (item.GetType() == typeof(ScrollMenuFunctionItem))
                    {
                        // Function item
                        ScrollMenuFunctionItem functionItem = item as ScrollMenuFunctionItem;
                        d3dBase.DrawShadowText(functionItem.Text, baseFont, textX + 3, textYOffset, TextColor);
                    }

                    textYOffset += ItemHeight;
                }
            }
        }

        public void Draw1()
        {
            if (!IsVisible)
                return;

            // Visibility checks
            DoVisibilityChecks();

            // Background
            float boxWidth = 300;
            float boxHeight = CalculateMenuHeight() + 10;
            float boxX = PositionX - (boxWidth / 2);
            float boxY = PositionY - (boxHeight / 2);
            d3dBase.DrawFilledBox(boxX, boxY, boxWidth, boxHeight, BoxBackground);

            // Title
            float titleHeight = 20;
            d3dBase.DrawFilledBox(boxX, boxY - titleHeight, boxWidth, titleHeight, TitleBackground);
            d3dBase.DrawCenterText(baseFont, "Quick Menu", new RectangleF(boxX, boxY - titleHeight, boxWidth, titleHeight), TextColor);

            // Vars
            float textYOffset = PositionY - (boxHeight / 2);
            float textX = PositionX - (boxWidth / 2);

            // Items
            for (int i = 0; i < Categories.Count; i++)
            {
                ScrollMenuCategory category = Categories[i];

                // Check if at least one item in list is visible
                if (!category.IsVisible)
                    continue;

                string categoryTitle = string.Format("--- {0} ---", category.Title);
                d3dBase.DrawCenterText(baseFont, categoryTitle, new RectangleF(textX, textYOffset, boxWidth, ItemHeight), Color.White);
                textYOffset += ItemHeight;

                for (int j = 0; j < category.Items.Count(); j++)
                {
                    ScrollMenuBaseItem item = category.Items[j];

                    if (!item.IsVisible)
                    {
                        if (j == selectedItemIndex && i == selectedCategoryIndex)
                            MoveDownInMenu();

                        continue;
                    }
                        

                    if (j == selectedItemIndex && i == selectedCategoryIndex)
                    {
                        MarkerColor.A = 150;
                        d3dBase.DrawFilledBox(textX, textYOffset, boxWidth, ItemHeight, MarkerColor);
                    }

                    if (item.GetType() == typeof(ScrollMenuBoolItem))
                    {
                        // Bool item
                        ScrollMenuBoolItem boolItem = item as ScrollMenuBoolItem;
                        d3dBase.DrawShadowText(boolItem.Text, baseFont, textX + 3, textYOffset, TextColor);
                        string state = boolItem.State ? "ON" : "OFF";
                        Color stateColor = boolItem.State ? Color.Green : Color.Red;
                        d3dBase.DrawShadowText(state, baseFont, (boxX + boxWidth - 30), textYOffset, stateColor);
                    }
                    else if (item.GetType() == typeof(ScrollMenuFunctionItem))
                    {
                        // Function item
                        ScrollMenuFunctionItem functionItem = item as ScrollMenuFunctionItem;
                        d3dBase.DrawShadowText(functionItem.Text, baseFont, textX + 3, textYOffset, TextColor);
                    }

                    textYOffset += ItemHeight;
                }
            }
        }

        private void DoVisibilityChecks()
        {
            foreach (ScrollMenuCategory category in Categories)
            {
                foreach (ScrollMenuBaseItem item in category.Items)
                {
                    if(item.IsVisibleCheck != null)
                        item.IsVisibleCheck(item);
                }
            }
        }

        private float CalculateMenuHeight()
        {
            float height = 0;

            foreach (ScrollMenuCategory category in Categories)
            {
                if (category.IsVisible)
                    height += CategoryTitleHeight;

                height += category.Items.Where(i => i.IsVisible).Count() * ItemHeight;
            }

            return height;
        }

        public bool MouseEvent(int code, IntPtr wParam, IntPtr lParam)
        {
            if (!IsVisible)
                return false;

            if (code >= 0 && wParam == (IntPtr)WM_MOUSEWHEEL)
            {
                NativeStructs.MSLLHOOKSTRUCT hookStruct = (NativeStructs.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeStructs.MSLLHOOKSTRUCT));

                bool up = false;
                if ((int)hookStruct.mouseData > 0)
                    up = true;

                if (up)
                    SelectPreviousItem();
                else
                    SelectNextItem();

                return true;
            }
            else if(code >= 0 && wParam == (IntPtr)WM_MBUTTONUP)
            {
                if (!selectedItem.IsVisible)
                    return false;

                if (selectedItem.GetType() == typeof(ScrollMenuBoolItem))
                {
                    ScrollMenuBoolItem boolItem = selectedItem as ScrollMenuBoolItem;
                    boolItem.Change();
                }
                else if (selectedItem.GetType() == typeof(ScrollMenuFunctionItem))
                {
                    ScrollMenuFunctionItem functionItem = selectedItem as ScrollMenuFunctionItem;
                    functionItem.Call();
                }

                return false;
            }

            return false;
        }

        private void MoveDownInMenu()
        {
            if (Categories[selectedCategoryIndex].Items.Count - 1 > selectedItemIndex)
            {
                selectedItemIndex++;
            }
            else
            {
                if (Categories.Count - 1 > selectedCategoryIndex)
                    selectedCategoryIndex++;
                else
                    selectedCategoryIndex = 0;

                selectedItemIndex = 0;
            }
        }

        private void MoveUpInMenu()
        {
            if (selectedItemIndex > 0)
            {
                selectedItemIndex--;
            }
            else
            {
                if (selectedCategoryIndex > 0)
                    selectedCategoryIndex--;
                else
                    selectedCategoryIndex = Categories.Count - 1;

                selectedItemIndex = Categories[selectedCategoryIndex].Items.Count - 1;
            }
        }

        private void SelectPreviousItem()
        {
            List<ScrollMenuBaseItem> currentCategoryItems = selectedCategory.Items.Where(i => i.IsVisible).ToList();

            int currentItemIndex = selectedCategory.Items.Where(i => i.IsVisible).ToList().IndexOf(selectedItem);
            if (currentItemIndex == 0) // If the current item is the first in the list
            {
                // Select previous category
                List<ScrollMenuCategory> visibleCategories = Categories.Where(c => c.IsVisible).ToList();
                int currentCategoryIndex = visibleCategories.IndexOf(selectedCategory);
                if (currentCategoryIndex == 0)
                    selectedCategory = visibleCategories.Last();
                else
                    selectedCategory = visibleCategories[currentCategoryIndex - 1];

                selectedItem = selectedCategory.Items.Last(item => item.IsVisible);
            }
            else
            {
                // Select previous item
                selectedItem = currentCategoryItems[currentItemIndex - 1];
            }
        }

        private void SelectNextItem()
        {
            List<ScrollMenuBaseItem> currentCategoryItems = selectedCategory.Items.Where(i => i.IsVisible).ToList();

            int currentItemIndex = selectedCategory.Items.Where(i => i.IsVisible).ToList().IndexOf(selectedItem);
            if(currentItemIndex >= currentCategoryItems.Count - 1) // If the current item is the last in the list
            {
                // Select next category
                List<ScrollMenuCategory> visibleCategories = Categories.Where(c => c.IsVisible).ToList();
                int currentCategoryIndex = visibleCategories.IndexOf(selectedCategory);
                if (currentCategoryIndex == visibleCategories.Count - 1)
                    selectedCategory = visibleCategories[0];
                else
                    selectedCategory = visibleCategories[currentCategoryIndex + 1];

                selectedItem = selectedCategory.Items.FirstOrDefault(item => item.IsVisible);
            }
            else
            {
                // Select next item
                selectedItem = currentCategoryItems[currentItemIndex + 1];
            }
        }

        #endregion
    }
}
