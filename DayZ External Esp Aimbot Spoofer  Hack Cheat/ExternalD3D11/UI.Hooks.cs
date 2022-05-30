using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExternalD3D11
{
    public partial class UI
    {
        #region Globals

        // Keyboard hook
        private delegate IntPtr LowLevelHookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x0101;
        private LowLevelHookProc _keyboardProc = KeyboardHook;
        private static IntPtr _keyboardHookID = IntPtr.Zero;

        // Mouse hook
        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;
        private static LowLevelHookProc _mouseProc = MouseHook;
        private static IntPtr _mouseHookID = IntPtr.Zero;

        // Keys
        public bool W_Pressed;
        public bool Insert_Pressed;
        public bool Control_Pressed;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Methods

        public void SetHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            _keyboardHookID = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProc, hInstance, 0);
            //_mouseHookID = SetWindowsHookEx(WH_MOUSE_LL, _mouseProc, hInstance, 0);
        }

        public void UnHook()
        {
            UnhookWindowsHookEx(_keyboardHookID);
            //UnhookWindowsHookEx(_mouseHookID);
        }

        public static IntPtr MouseHook(int code, IntPtr wParam, IntPtr lParam)
        {

            NativeStructs.MSLLHOOKSTRUCT hookStruct = (NativeStructs.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeStructs.MSLLHOOKSTRUCT));

            bool up = false;
            if ((int)hookStruct.mouseData > 0)
                up = true;

            if (UI.CurrentInstane.OnMouseWheel != null)
                UI.CurrentInstane.OnMouseWheel(UI.CurrentInstane, up);

            return CallNextHookEx(_mouseHookID, code, (int)wParam, lParam);
        }

        public static int HiWord(int number)
        {
            if ((number & 0x80000000) == 0x80000000)
                return (number >> 16);
            else
                return (number >> 16) & 0xffff;
        }


        public static IntPtr KeyboardHook(int code, IntPtr wParam, IntPtr lParam)
        {
            int vkCode = Marshal.ReadInt32(lParam);

            if (vkCode == Keys.Divide.GetHashCode() && wParam == (IntPtr)WM_KEYUP)
                UI.CurrentInstane.Console.IsVisible = !UI.CurrentInstane.Console.IsVisible;

            if (UI.CurrentInstane.Console.IsVisible)
            {
                if (!UI.CurrentInstane.Console.KeyPress(vkCode, wParam, lParam))
                    return CallNextHookEx(_keyboardHookID, code, (int)wParam, lParam);
                else
                    return (IntPtr)1;
            }

            if (wParam == (IntPtr)WM_KEYUP)
            {
                if (UI.CurrentInstane.OnKeyPressed != null)
                    UI.CurrentInstane.OnKeyPressed(UI.CurrentInstane, vkCode);
            }

            if (UI.CurrentInstane.TextBox.IsVisible)
            {
                if (!UI.CurrentInstane.TextBox.KeyPress(vkCode, wParam, lParam))
                    return CallNextHookEx(_keyboardHookID, code, (int)wParam, lParam);
                else
                    return (IntPtr)1;
            }

            // W Pressed
            if (vkCode == Keys.W.GetHashCode() && wParam == (IntPtr)WM_KEYUP)
                UI.CurrentInstane.W_Pressed = false;
            if (vkCode == Keys.W.GetHashCode() && wParam == (IntPtr)WM_KEYDOWN)
                UI.CurrentInstane.W_Pressed = true;

            // Control Pressed
            if (vkCode == Keys.LControlKey.GetHashCode() && wParam == (IntPtr)WM_KEYUP)
                UI.CurrentInstane.Control_Pressed = false;
            if (vkCode == Keys.LControlKey.GetHashCode() && wParam == (IntPtr)WM_KEYDOWN)
                UI.CurrentInstane.Control_Pressed = true;

            // Insert Pressed
            if (vkCode == Keys.Insert.GetHashCode() && wParam == (IntPtr)WM_KEYUP)
                UI.CurrentInstane.Insert_Pressed = false;
            if (vkCode == Keys.Insert.GetHashCode() && wParam == (IntPtr)WM_KEYDOWN)
                UI.CurrentInstane.Insert_Pressed = true;

            if (!UI.CurrentInstane.Menu.KeyPress(vkCode, wParam, lParam))
                return CallNextHookEx(_keyboardHookID, code, (int)wParam, lParam);

            return (IntPtr)1;
        }

        #endregion
    }
}
