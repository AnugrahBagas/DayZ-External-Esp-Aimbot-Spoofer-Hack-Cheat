using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontFactory = SharpDX.DirectWrite.Factory;
using D2DFactory = SharpDX.Direct2D1.Factory;
using System.Diagnostics;

namespace ExternalD3D11
{
    public partial class D3DOverlay : RenderForm
    {
        #region Globals

        private MarginR margin;
        internal struct MarginR { public int Left, Right, Top, Bottom; }

        private HwndRenderTargetProperties renderProperties;

        #endregion

        #region Properties

        public FontFactory FontBase { get; set; }
        public WindowRenderTarget Device { get; set; }
        public D2DFactory Factory { get; set; }

        #endregion

        #region Events

        public delegate void OnOverlayLoadedEventHandler(object sender);
        public event OnOverlayLoadedEventHandler OnOverlayLoaded;

        #endregion

        #region Dll Imports

        [DllImport("user32.dll", SetLastError = true)]
        private static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("dwmapi.dll")]
        static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MarginR pMargins);


        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        #region Methods

        public D3DOverlay()
        {
            // Form style properties
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "D3DForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.DoubleBuffered = true;
            this.Load += D3DOverlay_Load;

            // Title
            this.Text = GetUniqueKey(20);

            const int GWL_EXSTYLE = -20;
            const int WS_EX_LAYERED = 0x80000;
            const int WS_EX_TRANSPARENT = 0x20;
            const int LWA_ALPHA = 0x2;

            //Make the window's border completely transparant
            SetWindowLong(this.Handle, GWL_EXSTYLE, (IntPtr)(GetWindowLong(this.Handle, GWL_EXSTYLE) ^ WS_EX_LAYERED ^ WS_EX_TRANSPARENT));

            //Set the Alpha on the Whole Window to 255 (solid)
            SetLayeredWindowAttributes(this.Handle, 0, 255, LWA_ALPHA);

            Factory = new D2DFactory();
            FontBase = new FontFactory();
            renderProperties = new HwndRenderTargetProperties()
            {
                Hwnd = this.Handle,
                PixelSize = new Size2(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height),
                PresentOptions = PresentOptions.None
            };

            //Init DirectX
            Device = new WindowRenderTarget(Factory, new RenderTargetProperties(new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), renderProperties);
            Device.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;// you can set another text mode
        }

        private void D3DOverlay_Load(object sender, EventArgs e)
        {
            if (OnOverlayLoaded!= null)
                OnOverlayLoaded(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Create a margin (the whole form)
            margin.Left = 0;
            margin.Top = 0;
            margin.Right = this.Width;
            margin.Bottom = this.Height;

            //Expand the Aero Glass Effect Border to the WHOLE form.
            // since we have already had the border invisible we now
            // have a completely invisible window - apart from the DirectX
            // renders NOT in black.
            DwmExtendFrameIntoClientArea(this.Handle, ref margin);
        }

        public void BeginDraw()
        {
            Device.BeginDraw();
            Device.Clear(Color.Transparent);
        }

        public void EndDraw()
        {
            Device.EndDraw();
        }

        private static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzåäöABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ1234567890АаБбВвГгДдЕеӘәЖжЗзИиЙйКкЛлМмНнОоÖöПпПпрСсТтуФфХхҺһҺһЧч'ШшьЭэԚԜԝ".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public void FitGameToScreen(string processName)
        {
            const short SWP_NOZORDER = 0x4;
            const int SWP_SHOWWINDOW = 0x0040;
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Count() > 0)
            {
                string gameTitle = processes[0].MainWindowTitle;
                IntPtr hWnd = FindWindowByCaption(IntPtr.Zero, gameTitle);
                SetWindowPos(hWnd, 0, -8, -30, Screen.PrimaryScreen.WorkingArea.Width + 16, Screen.PrimaryScreen.WorkingArea.Height + 38, SWP_NOZORDER | SWP_SHOWWINDOW);
                //GetWindowRect(hWnd, out _gameWindowRectangle);
            }
        }

        public new void Dispose()
        {
            Factory.Dispose();
            FontBase.Dispose();
            Device.Dispose();
        }

        #endregion
    }
}
