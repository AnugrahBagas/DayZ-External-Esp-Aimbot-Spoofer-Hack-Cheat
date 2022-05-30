using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.ScrollMenu
{
    public class ScrollMenuBaseItem
    {
        #region Properties

        public string Text { get; set; }
        public Action<ScrollMenuBaseItem> IsVisibleCheck { get; set; }
        public bool IsVisible { get; set; }

        #endregion

        #region Constructor

        public ScrollMenuBaseItem()
        {
        }

        public ScrollMenuBaseItem(string text)
        {
            IsVisible = true;
            Text = text;
        }

        public ScrollMenuBaseItem(string text, Action<ScrollMenuBaseItem> isVisibleCheck)
        {
            IsVisible = true;
            Text = text;
            IsVisibleCheck = isVisibleCheck;
        }

        #endregion
    }
}
