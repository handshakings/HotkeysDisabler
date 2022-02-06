using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace HotkeyDisableService
{
    public class Worker : BackgroundService
    {
        bool ctrl, shift, windows, alt, delete, tab, d, l, right, left;
        KeyHelper kh;

        public Worker()
        {
            kh = new KeyHelper();
            kh.KeyDown += Kh_KeyDown;
            kh.KeyUp += Kh_KeyUp;
        }
        private void Kh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey) ctrl = true;
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) shift = true;
            if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) windows = true;
            if (e.KeyCode == Keys.Alt || e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu) alt = true;
            if (e.KeyCode == Keys.Delete) delete = true;
            if (e.KeyCode == Keys.Tab) tab = true;
            if (e.KeyCode == Keys.D) d = true;
            if (e.KeyCode == Keys.L) l = true;
            if (e.KeyCode == Keys.Right) right = true;
            if (e.KeyCode == Keys.Left) left = true;

            //if(windows && l)
            //{
            //    textBox1.Text = windows+" "+l;
            //    e.Handled = true;
            //}

            if ((windows && tab) || (alt && tab) || (windows && ctrl && d) || (windows && ctrl && right) || (windows && ctrl && left) || (windows && l) || (ctrl && alt && delete))
            {
                //textBox1.Text = "Ctrl:" + ctrl + "\nShift:" + shift + "\nwindows:" + windows + "\nalt" + alt + "\nDelete:" + delete + "\ntab:" + tab + "\nright:" + right + "\nleft:" + left;
                //MessageBox.Show(windows + "  " + l);
                e.Handled = true;
            }
        }
        private void Kh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey) ctrl = false;
            if (e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) shift = false;
            if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin) windows = false;
            if (e.KeyCode == Keys.Alt || e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu) alt = false;
            if (e.KeyCode == Keys.Delete) delete = false;
            if (e.KeyCode == Keys.Tab) tab = false;
            if (e.KeyCode == Keys.D) d = false;
            if (e.KeyCode == Keys.L) l = false;
        }

        

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
        }
        public override void Dispose(){}



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

        }
    }
}
