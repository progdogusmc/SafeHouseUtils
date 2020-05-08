using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restart_Voicemail
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = @"stop SafeHouseVoiceMail_svc";
            info.FileName = @"c:\windows\system32\net.exe";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            using (Process shutdownProcess = Process.Start(info))
            {
                while (!shutdownProcess.WaitForExit(100))
                {
                    if (!shutdownProcess.Responding)
                    {
                        break;
                    }
                }
            }

            Thread waitForShutdownThread = new Thread(waitForShutdown);
            waitForShutdownThread.Start();
        }

        private void setStatusText(string text)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => { setStatusText(text); });
                return;
            }

            StatusTextBlock.Text = text;
        }

        private void waitForShutdown()
        {
            DateTime startTime = DateTime.Now;
            DateTime elapsedTime = DateTime.Now;
            while ((elapsedTime - startTime).TotalSeconds < 90)
            {
                elapsedTime = DateTime.Now;
                setStatusText(string.Format("Voicemail shutting down: {0} seconds left", (90 - (int)(elapsedTime - startTime).TotalSeconds)));
                Thread.Sleep(500);
            }
            
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = @"start SafeHouseVoiceMail_svc";
            info.FileName = @"c:\windows\system32\net.exe";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            using (Process resetProcess = Process.Start(info))
            {
                while (!resetProcess.WaitForExit(100))
                {
                    if (!resetProcess.Responding)
                    {
                        break;
                    }
                }
            }

            Thread waitForStartupThread = new Thread(waitForStartup);
            waitForStartupThread.Start();
        }

        private void waitForStartup()
        {
            DateTime startTime = DateTime.Now;
            DateTime elapsedTime = DateTime.Now;
            while ((elapsedTime - startTime).TotalSeconds < 120)
            {
                elapsedTime = DateTime.Now;
                setStatusText(string.Format("Voicemail restarting: {0} seconds left", (120 - (int)(elapsedTime - startTime).TotalSeconds)));
                Thread.Sleep(500);
            }

            Dispatcher.Invoke(() => { Application.Current.Shutdown(); });
        }
    }
}
