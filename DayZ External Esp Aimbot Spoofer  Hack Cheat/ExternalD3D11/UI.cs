using D3DMenu;
using ExternalD3D11.ClickableMenu;
using ExternalD3D11.Console;
using ExternalD3D11.Controls;
using SharpDX;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ExternalD3D11
{
    public partial class UI
    {
        #region Globals

        public static UI CurrentInstane;

        private DirectXBase d3dBase;


        // Locks
        private Object drawTextLock = new Object();
        private Object drawBoxLock = new Object();
        private Object drawLineLock = new Object();
        private Object drawFilledBoxLock = new Object();
        private Object drawCircleLock = new Object();

        // Fonts
        public TextFormat BaseFont;
        public TextFormat UIFont;
        public TextFormat UIFontSmall;
        public TextFormat HeadlineFont;
        public TextFormat BoldFont;
        public TextFormat SeparatorFont;
        public TextFormat WarningFont;
        public TextFormat CreditText;

        // FPS
        private FpsRenderer fps;

        // Player Count bling bling
        private System.Timers.Timer _playerCountWarningTimer = new System.Timers.Timer();
        private int _playerCountBlinks = 0;
        private bool _playerCountWarningTimerIsActive = false;
        public Color PlayerCountColor = Color.White;

        // Console
        public D3DConsole Console;

        // Textbox
        public D3DTextBox TextBox;

        // Clickable Menu
        public D3DClickableArea ClickableArea;

        #endregion

        #region Events

        public delegate void OnOverlayLoadedEventHandler(object sender);
        public event OnOverlayLoadedEventHandler OnOverlayLoaded;

        public delegate void OnKeyPressedEventHandler(object sender, int code);
        public event OnKeyPressedEventHandler OnKeyPressed;

        public delegate void OnMouseWheelEventHandler(object sender, bool up);
        public event OnMouseWheelEventHandler OnMouseWheel;

        #endregion

        #region Properties

        public DirectXMenu Menu { get; set; }
        public double FPS { set;  get; }
        public int Width { get { return d3dBase.Overlay.Width; } }
        public int Height { get { return d3dBase.Overlay.Height; } }

        public RawMatrix3x2 DeviceTransform { get { return d3dBase.DeviceTransform; } set { d3dBase.DeviceTransform = value; } }

        #endregion

        #region Dll Imports

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);


        #endregion

        #region Constructor

        public UI()
        {
            CurrentInstane = this;

            // Base
            d3dBase = new DirectXBase();
            d3dBase.OnOverlayLoaded += D3dBase_OnOverlayLoaded;

            Console = new D3DConsole(d3dBase);
            TextBox = new D3DTextBox(d3dBase);
            ClickableArea = new D3DClickableArea(d3dBase);

            // Fonts
            BaseFont = new TextFormat(d3dBase.FontBase, "Lucida Sans", 13f); // Gadugi, Lucida Sans, Segoue UI, Terminal
            UIFont = new TextFormat(d3dBase.FontBase, "Gadugi", FontWeight.Bold, FontStyle.Normal, 13);
            UIFontSmall = new TextFormat(d3dBase.FontBase, "Gadugi", FontWeight.Normal, FontStyle.Normal, 13);
            HeadlineFont = new TextFormat(d3dBase.FontBase, "Verdana", FontWeight.Bold, FontStyle.Normal, 12f);
            BoldFont = new TextFormat(d3dBase.FontBase, "Lucida Sans", FontWeight.Bold, FontStyle.Normal, 12f);
            SeparatorFont = new TextFormat(d3dBase.FontBase, "Lucida Sans", FontWeight.Regular, FontStyle.Normal, 10f);
            WarningFont = new TextFormat(d3dBase.FontBase, "Lucida Sans", FontWeight.Bold, FontStyle.Normal, 16);
            CreditText = new TextFormat(d3dBase.FontBase, "Museo", 8);

        }

        #endregion

        #region Base Methods

        public void Initialize()
        {
            // Keyboard hook
            SetHook();

            fps = new FpsRenderer();
            fps.Initialize();

            ClickableArea.Initialize();

            Application.Run(d3dBase.Overlay); // Blocking
        }

        private void D3dBase_OnOverlayLoaded(object sender)
        {
            if (OnOverlayLoaded != null)
                OnOverlayLoaded(this);
        }

        public void BeginDraw()
        {
            d3dBase.BeginDraw();
            FPS = fps.Calc();

            //Console.Draw();
            //TextBox.Draw();
            //ClickableArea.Draw();
        }

        public void EndDraw()
        {
            d3dBase.EndDraw();
        }

        public void FitGameToScreen(string processName)
        {
            d3dBase.Overlay.FitGameToScreen(processName);
        }

        public void Dispose()
        {
            BaseFont.Dispose();
            UIFont.Dispose();
            HeadlineFont.Dispose();
            BoldFont.Dispose();
            SeparatorFont.Dispose();
            d3dBase.Dispose();
            ClickableArea.Dispose();
            UnHook();
        }

        #endregion

        #region Advanced Drawing

        public void DrawFPS()
        {
            lock (drawTextLock)
            {
                DrawShadowText(UIFont, string.Format("{0:F2}", FPS), Width - 80, 20, Color.White);
            }
        }

        public void DrawWarningBox(string textTitle, string subText, float y, int transparency)
        {
            Size2F titleSize = MeasureText(textTitle, WarningFont);
            Size2F subTextSize = MeasureText(subText, BaseFont);

            Size2F biggerTextSize;
            if (titleSize.Width > subTextSize.Width)
                biggerTextSize = titleSize;
            else
                biggerTextSize = subTextSize;


            float boxWidthPlus = 20;
            float box_h = 45;
            float box_x = (Width / 2) - (biggerTextSize.Width / 2) - (boxWidthPlus / 2);
            float box_y = (int)y;

            DrawBox(box_x, box_y, biggerTextSize.Width + boxWidthPlus, box_h, 1, Color.Black);
            DrawFilledBox(box_x, box_y, biggerTextSize.Width + boxWidthPlus, box_h, new Color(60, 60, 60, transparency));

            float textX = (Width / 2) - (biggerTextSize.Width / 2);

            RectangleF titleRect = new RectangleF(textX, box_y + 3, biggerTextSize.Width, titleSize.Height);
            RectangleF subTextRect = new RectangleF(textX, box_y + 25, biggerTextSize.Width, subTextSize.Height);


            DrawBaseText(WarningFont, textTitle, titleRect, new Color(205, 19, 19));
            DrawBaseText(BaseFont, subText, subTextRect, new Color(206, 206, 206));
        }

        public void DrawProgressbar(string title, float x, float y, double maxValue, double value)
        {
            float totalHeight = 20;
            float totalWidth = 250;
            float textFieldWidth = 50;
            float textX = x + 5;
            float textY = y;

            // Corner
            //DrawBox(x, y, totalWidth, totalHeight, 1, Color.Black);

            // Left box
            DrawFilledBox(x, y, textFieldWidth, totalHeight, new Color(40, 40, 40));
            DrawBaseText(BaseFont, title, textX, textY, Color.White);

            // MainBox
            float mainBoxX = x + textFieldWidth;
            float mainBoxY = y;
            float mainBoxWidth = totalWidth - textFieldWidth;
            double percentageValue = Math.Ceiling(((value * 100) / maxValue));

            DrawFilledBox(mainBoxX, mainBoxY, mainBoxWidth, totalHeight, new Color(104, 104, 104));

            // Calculate how much the progressbar is filled
            int coloredWidth = (int)((percentageValue / 100) * mainBoxWidth);

            Color fillColor = Color.DimGray;
            if (percentageValue > 80)
                fillColor = Color.DarkGreen;
            else if (percentageValue > 50)
                fillColor = Color.Gold;
            else if (percentageValue > 20)
                fillColor = Color.Orange;
            else if (percentageValue > 0)
                fillColor = Color.DarkRed;

            if (coloredWidth > 0)
                DrawFilledBox(mainBoxX, mainBoxY, coloredWidth, totalHeight, fillColor);

            // Draw Text
            string valueText = value.ToString("0") + " (" + percentageValue.ToString("00") + "%)";
            RectangleF valueTextRect = new RectangleF(mainBoxX, mainBoxY, mainBoxWidth, totalHeight);

            DrawCenterText(BaseFont, valueText, valueTextRect, Color.White);
        }

        public void DrawStatusBox(string title, float x, float y, string value)
        {
            float totalHeight = 20;
            float totalWidth = 160;
            float textFieldWidth = 90;
            float textX = x + 5;
            float textY = y + 2;

            // Corner
            //DrawBox(x, y, totalWidth, totalHeight, 1, Color.Black);

            // Left box
            DrawFilledBox(x, y, textFieldWidth, totalHeight, new Color(40, 40, 40));
            DrawBaseText(BaseFont, title, textX, textY, Color.White);

            // MainBox
            float mainBoxX = x + textFieldWidth;
            float mainBoxY = y;
            float mainBoxWidth = totalWidth - textFieldWidth;

            DrawFilledBox(mainBoxX, mainBoxY, mainBoxWidth, totalHeight, new Color(104, 104, 104));

            // Draw Text
            RectangleF valueTextRect = new RectangleF((int)mainBoxX, (int)mainBoxY + 2, mainBoxWidth, totalHeight);
            DrawCenterText(BaseFont, value, valueTextRect, Color.White);
        }

        public void ShowPlayerCountWarning()
        {
            if (!_playerCountWarningTimerIsActive)
            {
                _playerCountBlinks = 0;
                _playerCountWarningTimer = new System.Timers.Timer();
                _playerCountWarningTimer.Interval = 150;
                _playerCountWarningTimer.Elapsed += _playerCountWarningTimer_Elapsed;
                _playerCountWarningTimerIsActive = true;
                _playerCountWarningTimer.Enabled = true;
            }
        }

        private void _playerCountWarningTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (PlayerCountColor == Color.White)
                PlayerCountColor = Color.Red;
            else
                PlayerCountColor = Color.White;

            _playerCountBlinks++;

            if (_playerCountBlinks >= 16)
            {
                _playerCountWarningTimer.Enabled = false;
                _playerCountWarningTimerIsActive = false;
            }
        }



        #endregion
    }
}
