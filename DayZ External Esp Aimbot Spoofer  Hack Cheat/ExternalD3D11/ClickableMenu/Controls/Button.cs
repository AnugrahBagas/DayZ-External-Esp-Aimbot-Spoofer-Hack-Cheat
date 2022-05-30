using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.ClickableMenu.Controls
{
    public class Button : BaseControl
    {
        #region Globals

        private bool isLeftMouseDown = false;

        #endregion

        #region Events

        public delegate void OnClickEventHandler(object sender, EventArgs e);
        public event OnClickEventHandler OnClick;

        #endregion

        #region Properties

        public string Text { get; set; }

        // Style
        public float BorderRadius { get; set; }

        // Colors
        public Color Background { get; set; }
        public Color Border { get; set; }
        public Color Foreground { get; set; }
        public Color HoverBackground { get; set; }
        public Color HoverBorder { get; set; }
        public Color HoverForeground { get; set; }
        public Color ClickBackground { get; set; }
        public Color ClickBorder { get; set; }
        public Color ClickForeground { get; set; }

        #endregion

        #region Constructor

        public Button(TextFormat font)
            :base(font)
        {
            BorderRadius = 2;

            Background = new Color(60, 60, 60);
            Border = new Color(40, 40, 40);
            Foreground = Color.White;
            HoverBackground = new Color(50, 50, 50);
            HoverBorder = new Color(20, 20, 20);
            HoverForeground = Color.White;
            ClickBackground = new Color(20, 20, 20);
            ClickBorder = new Color(20, 20, 20);
            ClickForeground = Color.White;
        }

        #endregion

        #region Methods

        public override void Calculate()
        {
            base.Calculate();

        }

        public override void Draw()
        {
            base.Draw();

            Color background;
            Color foreground;
            Color border;

            if (IsMouseOver)
            {
                if (isLeftMouseDown)
                {
                    background = ClickBackground;
                    border = ClickBorder;
                    foreground = ClickForeground;
                }
                else
                {
                    background = HoverBackground;
                    border = HoverBorder;
                    foreground = HoverForeground;
                }
            }
            else
            {
                background = Background;
                border = Border;
                foreground = Foreground;
            }

            d3dBase.DrawFilledRoundedBox(X, Y, Width, Height, BorderRadius, background);
            d3dBase.DrawCenterText(DefaultFont, Text, Boundings, foreground);
            d3dBase.DrawRoundedBox(X, Y, Width, Height, BorderRadius, border);
        }

        public override void OnMouseLeave()
        {
            //System.Console.WriteLine(Text + " mouse leave");
        }

        public override void OnMouseLeft(bool isDown)
        {
            if (isDown)
            {
                isLeftMouseDown = true;
                return;
            }
            else
            {
                isLeftMouseDown = false;
            }
            

            System.Console.WriteLine(Text + " clicked!");
        }

        #endregion
    }
}
