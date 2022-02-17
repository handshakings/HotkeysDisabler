using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoHotKeysDisabler
{
    public class KeyHelper
    {
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;


        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public Keys key;
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }


        [DllImport("user32.dll")]
        //id: hook type         2nd parameter is pointer to callback function, also called hook procedure
        //hMode: handle to the DLL containing the hook procedure 
        //dwThreadId: Thread ID with which the hook procedure is associated
        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, int wp, IntPtr lp);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string name);
        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hook);

        //hook type id
        const int WH_KEYBOARD_LL = 13;
        private delegate IntPtr LowLevelKeyboardProc(int nCode, int wParam, IntPtr lParam);
        private LowLevelKeyboardProc keyboardProcess;
        public static IntPtr ptrHook;
        public event KeyEventHandler KeyUp;
        public event KeyEventHandler KeyDown;

        public KeyHelper()
        {
            string objCurrentModuleName = Process.GetCurrentProcess().MainModule.ModuleName;
            keyboardProcess = new LowLevelKeyboardProc(CaptureKey);
            //ptrHook is handle to the hook procedure (keyborPress)
            ptrHook = SetWindowsHookEx(WH_KEYBOARD_LL, keyboardProcess, GetModuleHandle(objCurrentModuleName), 0);
        }

        private IntPtr CaptureKey(int nCode, int wp, IntPtr lp)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT keyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lp, typeof(KBDLLHOOKSTRUCT));
                KeyEventArgs eventArgs = new KeyEventArgs(keyInfo.key);

                if ((wp == WM_KEYDOWN || wp == WM_SYSKEYDOWN) && KeyDown != null && keyInfo.vkCode != 0)
                {
                    KeyDown(this, eventArgs);
                }
                else if ((wp == WM_KEYUP || wp == WM_SYSKEYUP) && (KeyUp != null) && keyInfo.vkCode != 0)
                {
                    KeyUp(this, eventArgs);
                }
                if (eventArgs.Handled)
                    return (IntPtr)1;
                else
                    return CallNextHookEx(ptrHook, nCode, wp, lp);
            }
            else
            {
                return CallNextHookEx(ptrHook, nCode, wp, lp);
            }
        }




        ~KeyHelper()
        {
            UnhookWindowsHookEx(ptrHook);
        }

    }

}
