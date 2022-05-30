using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.ScrollMenu
{
    public class ScrollMenuFunctionItem : ScrollMenuBaseItem
    {
        public Action Function { get; set; }

        public ScrollMenuFunctionItem(string text, Action function)
            :base(text)
        {
            Function = function;
        }

        public ScrollMenuFunctionItem(string text, Action function, Action<ScrollMenuBaseItem> isVisibleCheck)
            :base(text, isVisibleCheck)
        {
            Function = function;
        }

        public void Call()
        {
            Function();
        }
    }
}
