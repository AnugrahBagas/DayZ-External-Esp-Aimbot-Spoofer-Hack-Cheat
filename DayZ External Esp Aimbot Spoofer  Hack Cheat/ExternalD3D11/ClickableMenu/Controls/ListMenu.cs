using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.ClickableMenu.Controls
{
    public class ListMenu : BaseControl
    {
        #region Globals


        #endregion

        #region Properties

        public List<ListMenuItem> ListItems { get; set; }

        public Color Background { get; set; }
        
        #endregion

        #region Constructor

        public ListMenu(TextFormat font)
            :base(font)
        {
            ListItems = new List<ListMenuItem>();

            Width = 200;

            // Style
            Background = new Color(30, 30, 30, 150);
        }

        #endregion

        #region Methods

        public override void Initialize(DirectXBase d3d)
        {
            base.Initialize(d3d);
            foreach (ListMenuItem item in ListItems)
                item.Initialize(d3d);
        }

        public override void Calculate()
        {
            float totalHeight = 0;
            float itemYOffset = 0;

            foreach (ListMenuItem item in ListItems)
            {
                item.Calculate();
                totalHeight += item.Height;

                item.Width = Width;
                item.Y = itemYOffset + Y;
                item.X = X;
                itemYOffset += item.Height;
            }

            Height = totalHeight;
            Boundings = new RectangleF(X, Y, Width, totalHeight);
        }

        public override void Draw()
        {
            // Background
            d3dBase.DrawFilledBox(X, Y, Width, Height, Background);

            // Items
            foreach (ListMenuItem item in ListItems)
                item.Draw();
        }

        public override void OnMouseLeft(bool isDown)
        {
            base.OnMouseLeft(isDown);

            ListMenuItem listItem = ListItems.FirstOrDefault(i => i.IsMouseOver);
            listItem.OnMouseLeft(isDown);
        }

        public override void OnMouseMove(float x, float y)
        {
            base.OnMouseMove(x, y);

            for (int i = 0; i < ListItems.Count; i++)
            {
                BaseControl control = ListItems[i];
                Task.Run(delegate
                {
                    bool isInControl = control.Boundings.Contains(x, y);

                    if (isInControl && !control.IsMouseOver)
                    {
                        control.IsMouseOver = true;
                        control.OnMouseOver();
                    }
                    else if (!isInControl && control.IsMouseOver)
                    {
                        control.IsMouseOver = false;
                        control.OnMouseLeave();
                    }
                }
                );
            }
        }

        #endregion
    }
}
