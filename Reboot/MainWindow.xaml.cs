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

namespace Reboot
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
            Thread rebootThread = new Thread(reboot);
            rebootThread.Start();
        }

        private void reboot()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = @"/im mstsc.exe /f";
            info.FileName = @"c:\windows\system32\taskkill.exe";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            using (Process tscKillProcess = Process.Start(info))
            {
                while (!tscKillProcess.WaitForExit(100))
                {
                    if (!tscKillProcess.Responding)
                    {
                        break;
                    }
                }
            }

            info = new ProcessStartInfo();
            info.Arguments = @"/im admin.exe /f";
            info.FileName = @"c:\windows\system32\taskkill.exe";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            using (Process adminKillProcess = Process.Start(info))
            {
                while (!adminKillProcess.WaitForExit(100))
                {
                    if (!adminKillProcess.Responding)
                    {
                        break;
                    }
                }
            }

            info = new ProcessStartInfo();
            info.Arguments = @"/im entry.exe /f";
            info.FileName = @"c:\windows\system32\taskkill.exe";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            using (Process entryKillProcess = Process.Start(info))
            {
                while (!entryKillProcess.WaitForExit(100))
                {
                    if (!entryKillProcess.Responding)
                    {
                        break;
                    }
                }
            }

            info = new ProcessStartInfo();
            info.Arguments = @"/r /t 00";
            info.FileName = @"c:\windows\system32\shutdown.exe";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            Process.Start(info);
        }
    }
}
