using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.ClickableMenu.Controls
{
    public class BaseControl
    {
        #region Globals

        protected DirectXBase d3dBase;

        #endregion

        #region Properties

        public float Width { get; set; }
        public float Height { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public RectangleF Boundings { get; set; }
        public bool IsMouseOver { get; set; }

        public TextFormat DefaultFont { get; set; }


        #endregion

        #region Constructor

        public BaseControl(TextFormat font){ DefaultFont = font; }

        #endregion

        #region Methods

        public virtual void Initialize(DirectXBase d3d)
        {
            this.d3dBase = d3d;
        }

        public virtual void OnMouseOver() { }

        public virtual void OnMouseLeave() { }

        public virtual void OnMouseMove(float x, float y) { }

        public virtual void OnMouseLeft(bool isDown) { }

        public virtual void OnMouseRight(bool isDown) { }

        public virtual void OnMouseMiddle(bool isDown) { }

        public virtual void Calculate()
        {
            Boundings = new RectangleF(X, Y, Width, Height);
        }

        public virtual void Draw()
        {
            
        }

        #endregion
    }
}
