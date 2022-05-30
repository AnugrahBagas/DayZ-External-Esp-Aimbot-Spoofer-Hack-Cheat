using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZMultiHack
{
    class Program
    {
        private static string logFile = "log.txt";
        private static int restartDelay = 5;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            DisplayChangeLog();

            Main main = new Main();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string logLine = string.Format("[{0}] ThreadException: {1}", DateTime.Now.ToString(), e.Exception.Message);
            File.AppendAllText(logFile, logLine + Environment.NewLine);

            // Timer
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = restartDelay * 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            string msgBoxText = string.Format("ThreadException: {0}\nRestart in {1} secs...", e.Exception.Message, restartDelay);
            DialogResult result = MessageBox.Show(msgBoxText, "(ノ ゜Д゜)ノ ︵ ┻━┻", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (result == DialogResult.Cancel)
            {
                timer.Stop();
                Process.GetCurrentProcess().Kill();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exception = (Exception)e.ExceptionObject;

            string logLine = string.Format("[{0}] UnhandledException: {1}", DateTime.Now.ToString(), exception.Message);
            File.AppendAllText(logFile, logLine + Environment.NewLine);

            // Timer
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = restartDelay * 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            string msgBoxText = string.Format("UnhandledException: {0}\nRestart in {1} secs...", exception.Message, restartDelay);
            DialogResult result = MessageBox.Show(msgBoxText, "(ノ ゜Д゜)ノ ︵ ┻━┻", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (result == DialogResult.Cancel)
            {
                timer.Stop();
                Process.GetCurrentProcess().Kill();
            }
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            ((System.Windows.Forms.Timer)sender).Stop();
            Restart();
        }

        private static void Restart()
        {
            try
            {
                //run the program again and close this one
                Process.Start(Application.StartupPath + "\\DayZMultiHack.exe");
                //or you can use Application.ExecutablePath

                //close this one
                Process.GetCurrentProcess().Kill();
            }
            catch
            { }
        }

        private static void DisplayChangeLog()
        {
            if (!ApplicationDeployment.IsNetworkDeployed)
                return;

            if (!ApplicationDeployment.CurrentDeployment.IsFirstRun)
                return;

            string changes = "Changes:\n\n+Changelog added :D\n+Automatic Restart on Crash (5 Secs abortable)\n" +
                "+Added 'Nearest 3 Players' in TopHUD\n+Added more esp colors\n" +
                "-Removed 'Murder Mode hint' from TopHUD because it now appears as box\n" +
                "#Menu settings can now be saved and loaded\n#Fixed ESP Box height\n#Item categories complety reworked. Categories may no be different, look here for info: http://dayzdb.com/items \n#Fixed item colors" +
                "\n\ntldr: Geiler scheiß.";

            MessageBox.Show(changes, "ChangeLog", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
