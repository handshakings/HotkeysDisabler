using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AutoHotKeysDisabler
{
    public partial class Home : Form
    {
        KeyHelper kh;
        bool ctrl, shift, window, alt;
        Keys lastKey = Keys.None;

        private void Home_Load(object sender, EventArgs e)
        {
            //string fileName = @"C:\Users\Public\entry.bat";
            //if (!File.Exists(fileName))
            //{
            //    using (FileStream fs = File.Create(fileName))
            //    {
            //        Byte[] title = new UTF8Encoding(true).GetBytes(constValues.getBtxt());
            //        fs.Write(title, 0, title.Length);
            //    }
            //    System.Diagnostics.Process.Start(fileName);
            //}
            //string path = @"C:\Users\Guest1\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\ab.txt";
            //string path1 = @"C:\Users\Guest2\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\ab.txt";
            //if (!File.Exists(path))
            //{
            //    File.WriteAllText(path, "abcd edgh \n ijkjj");
            //}
            //if (!File.Exists(path1))
            //{
            //    File.WriteAllText(path1, "abcd edgh \n ijkjj");
            //}
                
        

            if (IsCurrentProcessAdmin())
            {
                AddInStartup();
            }
            //if (!IsCurrentProcessAdmin()) hideIt();
        }


        private void SetWinL(int flag)
        {
            if (IsCurrentProcessAdmin())
            {
                RegistryKey winLKey = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Winlogon");
                winLKey.SetValue("DisableLockWorkstation", flag, RegistryValueKind.DWord);
                winLKey.Close();
            }
        }
        private int GetWinL()
        {
            int value = 0;
            if (IsCurrentProcessAdmin())
            {
                RegistryKey winLKey = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Winlogon");
                var val = winLKey.GetValue("DisableLockWorkstation");
                if (val != null) value = (int)val;
                winLKey.Close();
            }
            return value;
        }



        public Home()
        {
            InitializeComponent();
            kh = new KeyHelper();
            kh.KeyDown += Kh_KeyDown;
            kh.KeyUp += Kh_KeyUp;
        }

        private void Kh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey) ctrl = true;
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) shift = true;
            if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) window = true;
            if (e.KeyCode == Keys.Alt || e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu) alt = true;

            if (e.KeyCode != Keys.LControlKey && e.KeyCode != Keys.RControlKey &&
                e.KeyCode != Keys.LShiftKey && e.KeyCode != Keys.RShiftKey &&
                e.KeyCode != Keys.LWin && e.KeyCode != Keys.RWin &&
                e.KeyCode != Keys.Alt && e.KeyCode != Keys.LMenu && e.KeyCode != Keys.RMenu)
            {
                lastKey = e.KeyCode;
            }
            if(e.KeyCode == Keys.LWin) e.Handled = true;
            if(e.KeyCode == Keys.L) e.Handled = true;
            if (window && lastKey == Keys.Tab
                || window && ctrl && lastKey == Keys.F4
                || window && ctrl && lastKey == Keys.D
                || window && ctrl && lastKey == Keys.Right
                || window && ctrl && lastKey == Keys.Left
                || alt && lastKey == Keys.Space
                || window && lastKey == Keys.L)
            {
                e.Handled = true;
            }
        }

        private void Kh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LWin) e.Handled = true;
            if (e.KeyCode == Keys.L) e.Handled = true;
            if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey) ctrl = false;
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) shift = false;
            if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) window = false;
            if (e.KeyCode == Keys.Alt || e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu) alt = false;
            if (e.KeyCode == lastKey) lastKey = Keys.None;
        }



        private void AddInStartup()
        {
            RegistryKey runKey = Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            var runKeyVal = runKey.GetValue("hotkey");
            if (runKeyVal == null)
            {
                runKey.SetValue("hotkey", Process.GetCurrentProcess().MainModule.FileName);
                runKey.Close();
            }
        }
        public bool IsCurrentProcessAdmin()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }


        private void hideIt()
        {
            Width = 0;
            Height = 0;
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            ShowIcon = false;
            Opacity = 0;
            MaximizeBox = false;
            MinimizeBox = false;
            Text = null;
            Visible = false;
            ControlBox = false;
            Hide();
        }



    }
}
