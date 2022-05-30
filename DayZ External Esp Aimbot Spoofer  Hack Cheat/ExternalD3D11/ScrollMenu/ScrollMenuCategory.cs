using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.ScrollMenu
{
    public class ScrollMenuCategory
    {
        public List<ScrollMenuBaseItem> Items { get; set; }
        public string Title { get; set; }

        public ScrollMenuCategory()
        {
            Items = new List<ScrollMenuBaseItem>();
            Title = string.Empty;
        }

        public bool IsVisible
        {
            get { return Items.Count(item => item.IsVisible) > 0; }
        }

    }
}
