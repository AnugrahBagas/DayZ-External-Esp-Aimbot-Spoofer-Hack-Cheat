using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.ClickableMenu.Controls
{
    public class ListMenuItem : BaseControl
    {
        #region Globals

        private Size2F TextSize;

        #endregion

        #region Properties

        public List<ListMenuItem> SubItems { get; set; }
        public string Text;

        // Style
        public float Padding;
        public Color Border;
        public Color Foreground;

        public Color Background;
        public Color HoverBackground;

        #endregion

        #region Constructor

        public ListMenuItem(TextFormat font)
            :base(font)
        {
            SubItems = new List<ListMenuItem>();

            //Style
            Padding = 5;
            Border = new Color(255, 117, 0);
            Foreground = Color.White;
            Background = new Color(30, 30, 30);
            HoverBackground = new Color(50, 50, 50);
            
        }

        #endregion

        #region Methods

        public override void Initialize(DirectXBase d3d)
        {
            base.Initialize(d3d);
            foreach (ListMenuItem item in SubItems)
                item.Initialize(d3d);
        }

        public override void Calculate()
        {
            Size2F textSize = d3dBase.MeasureText(Text, DefaultFont);
            this.Height = textSize.Height + (2 * Padding);
            Boundings = new RectangleF(X, Y, Width, Height);
        }

        public override void Draw()
        {
            Color background;

            if (IsMouseOver)
                background = HoverBackground;
            else
                background = Background;
            
            // Background
            d3dBase.DrawFilledBox(X, Y, Width, Height, background);

            // Border
            d3dBase.DrawBox(X, Y, Width, Height, 1, Border);

            // Text
            d3dBase.DrawText(Text, DefaultFont, X + Padding, Y + Padding, Foreground);

            // Check if there are sub items
            if(SubItems.Count > 0)
                d3dBase.DrawText("➤", DefaultFont, X + Width - Padding - 15, Y + Padding, Foreground);
        }

        public override void OnMouseLeft(bool isDown)
        {
            base.OnMouseLeft(isDown);

            if(!isDown)
                System.Console.WriteLine(Text + " clicked!");
        }

        #endregion
    }
}
