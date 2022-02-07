using System.Diagnostics;
using System.Reflection;

namespace HotkeysDisableWinFormApp
{
#nullable disable
    public partial class Home : Form
    {
        KeyHelper kh;
        string ctrl, shift, window, alt;
        string lastKey = null;
        string filePath = @"C:\Users\Public\\hotkeys.txt";

        string key1 = "Ctrl", key2 = null, key3;
        private void Home_Load(object sender, EventArgs e)
        {
            if (File.Exists(filePath))
            {
                comboBox1.SelectedIndex = 0;
                string[] hotkeysArray = File.ReadAllText(filePath).Split(",");
                foreach (string hotkey in hotkeysArray)
                {
                    listView1.Items.Add(hotkey);
                }
            }
        }
        private void radioButtonGroup1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                key1 = radioButton1.Text;
            }
            else if (radioButton2.Checked)
            {
                key1 = radioButton2.Text;
            }
            else if (radioButton3.Checked)
            {
                key1 = radioButton3.Text;
            }
            else if (radioButton4.Checked)
            {
                key1 = radioButton4.Text;
            }
            button1.Text = "Disable : " + GetHotkey();
        }
        private void radioButtonGroup2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                key2 = radioButton5.Text;
            }
            else if (radioButton6.Checked)
            {
                key2 = radioButton6.Text;
            }
            else if (radioButton7.Checked)
            {
                key2 = radioButton7.Text;
            }
            else if (radioButton8.Checked)
            {
                key2 = radioButton8.Text;
            }
            else if (radioButton9.Checked)
            {
                key2 = null;
            }
            button1.Text = "Disable : " + GetHotkey();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            key3 = comboBox1.Text;
            button1.Text = "Disable : " + GetHotkey();
        }
        private string GetHotkey()
        {
            string hotkey = null;
            if (key2 is not null)
            {
                hotkey = key1 + " + " + key2 + " + " + key3;
            }
            else if (key2 is null)
            {
                hotkey = key1 + " + " + key3;
            }
            return hotkey;
        }

  
        

        private void button1_Click(object sender, EventArgs e)
        {
            if(File.Exists(filePath))
            {
                string[] hotkeysArray = File.ReadAllText(filePath).Split(",");
                bool keyExist = false;
                foreach (string hotkey in hotkeysArray)
                {
                    if(hotkey == GetHotkey().Replace(" ", ""))
                    {
                        keyExist = true;
                    }
                }
                if(!keyExist)
                {
                    File.AppendAllText(filePath, ","+ GetHotkey().Replace(" ", ""));
                    listView1.Items.Add(GetHotkey().Replace(" ", ""));
                }  
            } 
            else
            {
                File.AppendAllText(filePath, GetHotkey().Replace(" ", ""));
                listView1.Items.Add(GetHotkey().Replace(" ", ""));
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(filePath) && listView1.SelectedItems.Count > 0)
            {
                List<string> hotkeysList = File.ReadAllText(filePath).Split(",").ToList();
                string selectedKey = listView1.SelectedItems[0].Text;
                hotkeysList.Remove(selectedKey);
                File.Delete(filePath);
                string data = string.Join(",", hotkeysList.ToArray());
                File.WriteAllText(filePath, data);

                listView1.SelectedItems[0].Remove();
            }
        }

        public Home()
        {
            InitializeComponent();
            kh = new KeyHelper();
            kh.KeyDown += Kh_KeyDown;
            kh.KeyUp += Kh_KeyUp;
            MakePersistant();
            
        }

        private void Kh_KeyDown(object sender, KeyEventArgs e)
        {
            if(File.Exists(filePath))
            {
                if (File.ReadAllText(filePath).Length > 4)
                {
                    if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey) ctrl = "Ctrl";
                    if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) shift = "Shift";
                    if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) window = "Window";
                    if (e.KeyCode == Keys.Alt || e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu) alt = "Alt";

                    if (e.KeyCode != Keys.LControlKey && e.KeyCode != Keys.RControlKey &&
                        e.KeyCode != Keys.LShiftKey && e.KeyCode != Keys.RShiftKey &&
                        e.KeyCode != Keys.LWin && e.KeyCode != Keys.RWin &&
                        e.KeyCode != Keys.Alt && e.KeyCode != Keys.LMenu && e.KeyCode != Keys.RMenu)
                    {
                        lastKey = e.KeyCode.ToString();
                    }

                    string[] hotkeysList = File.ReadAllText(filePath).Split(",");
                    foreach (string hotkey in hotkeysList)
                    {
                        string[] s = hotkey.Split("+");
                        string a = (ctrl + " " + shift + " " + alt + " " + window).Replace(" ", "");
                        if (s.Count() == 2)
                        {
                            if (a.Contains(s[0]) && lastKey == s[1])
                            {
                                e.Handled = true;
                            }
                        }
                        else if (s.Count() == 3)
                        {
                            if (a.Contains(s[0]) && a.Contains(s[1]) && lastKey == s[2])
                            {
                                e.Handled = true;
                            }
                        }
                    }
                }
            }
        }

        private void Kh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey) ctrl = null;
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) shift = null;
            if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) window = null;
            if (e.KeyCode == Keys.Alt || e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu) alt = null;
            if (e.KeyCode.ToString() == lastKey) lastKey = null;
        }



        private void MakePersistant()
        {
            string directory = Process.GetCurrentProcess().MainModule.FileName;
            Process cmd = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = @"/c reg ADD HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Run /v hotkey /f /d "+directory;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            cmd.StartInfo = startInfo;
            cmd.Start();
            cmd.Close();
        }
        private void Home_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
    }
}
