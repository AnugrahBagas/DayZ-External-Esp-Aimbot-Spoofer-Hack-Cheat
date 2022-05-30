using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternalD3D11.Controls
{
    public class D3DTextBox
    {
        #region Globals

        // Base
        private DirectXBase d3dBase;
        private TextFormat textBoxFont;

        // Caret
        private int caretTick = 0;
        private bool showCaret = true;
        private int caretPos = 0;

        // Text
        private string text = string.Empty;

        #endregion

        #region Properties

        public bool IsVisible = false;

        public float PositionX = 500;
        public float PositionY = 100;
        public float Width = 150;
        public float Height = 20;

        public float BorderThickness = 3;
        public float TextPaddingLeft = 5;

        public Color Background = Color.LightGray;
        public Color Foreground = Color.Black;

        public string Title = string.Empty;

        #endregion

        #region Events

        public delegate void OnEnterPressedEventHandler(object sender, string text);
        public event OnEnterPressedEventHandler OnEnterPressed;

        #endregion

        #region Constructor

        public D3DTextBox(DirectXBase directXBase)
        {
            d3dBase = directXBase;
            textBoxFont = new TextFormat(d3dBase.FontBase, "Consolas", 15f);
        }

        #endregion

        #region Methods

        public void Draw()
        {
            if (!IsVisible)
                return;

            // Calculations
            float textX = PositionX + BorderThickness + 3;
            float cursorBasePositionX = PositionX + 3;

            // Background
            float titleHeightAddition = 20;
            float padding = 5;
            float backgroundX = PositionX - padding;
            float backgroundY = PositionY - padding - titleHeightAddition;
            float backgroundWidth = Width + (padding * 2);
            float backgroundHeight = Height + titleHeightAddition + (padding * 2);
            d3dBase.DrawFilledBox(backgroundX, backgroundY, backgroundWidth, backgroundHeight, new Color(30, 30, 30, 200));

            // Title
            float titleX = backgroundX + padding;
            float titleY = backgroundY + padding;
            float titleWidth = backgroundWidth - (2 * padding);
            float titleHeight = backgroundHeight - (2 * padding);
            d3dBase.DrawCenterText(textBoxFont, Title, new RectangleF(titleX, titleY, titleWidth, titleHeight), Color.White);

            // TextBox
            d3dBase.DrawFilledBox(PositionX, PositionY, Width, Height, Background);

            // BaseText
            d3dBase.DrawText(text, textBoxFont, textX, PositionY, Foreground);

            // Caret
            caretTick += 1;
            if (caretTick >= 35)
            {
                caretTick = 0;
                showCaret = !showCaret;
            }

            if (showCaret)
                d3dBase.DrawText("|", textBoxFont, cursorBasePositionX + (caretPos * 8.3f), PositionY, Foreground);
        }

        public bool KeyPress(int code, IntPtr wParam, IntPtr lParam)
        {
            // If not keydown
            if (wParam != (IntPtr)0x100)
                return false;

            bool isUppercase = false;
            if (Control.ModifierKeys == Keys.Shift)
                isUppercase = true;

            char key = Convert.ToChar(code);
            if (isUppercase)
                key = char.ToUpper(key);
            else
                key = char.ToLower(key);


            if (char.IsLetter(key) || char.IsDigit(key) || key == Keys.Space.GetHashCode())
                AddToText(key);
            else if (code == Keys.Back.GetHashCode())
                BackKey();
            else if (code == Keys.Delete.GetHashCode())
                DeleteKey();
            else if (code == Keys.End.GetHashCode())
                MoveCaretToEnd();
            else if (code == Keys.Home.GetHashCode())
                MoveCaretToStart();
            else if (code == Keys.Left.GetHashCode())
                MoveCaretLeft();
            else if (code == Keys.Right.GetHashCode())
                MoveCaretRight();
            else if (code == Keys.Enter.GetHashCode())
                ProcessCommand();

            return true;
        }

        private void ProcessCommand()
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                ResetText();
                return;
            }

            if (OnEnterPressed != null)
                OnEnterPressed(this, text);
        }

        public void ResetText()
        {
            text = string.Empty;
            caretPos = 0;
        }

        private void DeleteKey()
        {
            if (text.Length > 0 && caretPos >= 0 && caretPos < text.Length)
                RemoveFromText(caretPos, 1, false);
        }

        private void BackKey()
        {
            if (text.Length > 0 && caretPos > 0)
                RemoveFromText(caretPos - 1, 1, true);
        }

        private void AddToText(char key)
        {
            if (text.Length + 1 > 70)
                return;

            text = text.Insert(caretPos, key.ToString().ToUpper());
            caretPos++;
        }

        private void RemoveFromText(int index, int count, bool removeFromCaret)
        {
            if (text.Length > 0 && caretPos >= 0)
            {
                text = text.Remove(index, count);

                if (removeFromCaret)
                    caretPos--;
            }
        }

        #region Caret position

        private void MoveCaretLeft()
        {
            if (caretPos > 0)
                caretPos--;
        }

        private void MoveCaretRight()
        {
            if (caretPos + 1 <= text.Length)
                caretPos++;
        }

        private void MoveCaretToEnd()
        {
            caretPos = text.Length;
        }

        private void MoveCaretToStart()
        {
            caretPos = 0;
        }

        #endregion

        #endregion
    }
}
