using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.ScrollMenu
{
    public class ScrollMenuBoolItem : ScrollMenuBaseItem
    {
        public bool State { get; set; }

        public ScrollMenuBoolItem(string text, bool state)
            :base(text)
        {
            Text = text;
            State = state;
        }

        public void Change()
        {
            State = !State;
        }
    }
}
