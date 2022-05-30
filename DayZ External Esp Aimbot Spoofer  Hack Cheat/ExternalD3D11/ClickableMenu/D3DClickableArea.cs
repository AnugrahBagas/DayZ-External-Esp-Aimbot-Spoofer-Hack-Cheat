using ExternalD3D11.ClickableMenu.Controls;
using SharpDX.DirectInput;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternalD3D11.ClickableMenu
{
    public class D3DClickableArea
    {
        #region Globals

        // Base
        private DirectXBase d3dBase;
        private TextFormat baseFont;
        private DirectInput directInput;

        private bool runUpdateLoop = true;

        // Mouse
        Mouse mouse;

        #endregion

        #region Properties

        public List<BaseControl> Controls { get; set; }

        #endregion

        #region Constrcutor

        public D3DClickableArea(DirectXBase directXBase)
        {
            d3dBase = directXBase;
            baseFont = new TextFormat(d3dBase.FontBase, "Lucida Sans", 13f);

            Controls = new List<BaseControl>();

            directInput = new DirectInput();
            mouse = new Mouse(directInput);
            mouse.Properties.BufferSize = 128;
            mouse.Acquire();
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            foreach (BaseControl control in Controls)
                control.Initialize(d3dBase);
        }

        private void Calculate()
        {
            // Calculate bounding boxes etz.
            for (int i = 0; i < Controls.Count; i++)
                Controls[i].Calculate();

            // Get Mouse events
            Task.Run(delegate { 
            MouseUpdate[] states = mouse.GetBufferedData();
            foreach (MouseUpdate state in states)
            {
                float cursorX = Cursor.Position.X;
                float cursorY = Cursor.Position.Y;

                if (state.IsButton)
                {
                    bool isDown = state.Value == 0 ? false : true;
                    BaseControl control = Controls.FirstOrDefault(c => c.IsMouseOver);

                    if (control != null)
                    {
                        switch (state.Offset)
                        {
                            case MouseOffset.Buttons0:
                                control.OnMouseLeft(isDown);
                                break;
                            case MouseOffset.Buttons1:
                                control.OnMouseRight(isDown);
                                break;
                            case MouseOffset.Buttons2:
                                control.OnMouseMiddle(isDown);
                                break;
                        }
                    }
                }
                else
                {
                    switch (state.Offset)
                    {
                        case MouseOffset.X:
                        case MouseOffset.Y:
                            CheckMouseOver(cursorX, cursorY);
                            break;
                        case MouseOffset.Z:
                            break;
                    }
                }
            }
            });
        }

        public void Draw()
        {
            Calculate();

            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Draw();
            }
        }

        private void CheckMouseOver(float x, float y)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                BaseControl control = Controls[i];
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

                        control.OnMouseMove(x, y);
                    }
                );
            }
        }

        public void Dispose()
        {
            runUpdateLoop = false;
            mouse.Unacquire();
            mouse.Dispose();
            directInput.Dispose();
        }

        #endregion
    }
}
