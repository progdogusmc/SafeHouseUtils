using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace Restart_Webguest
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
            info.FileName = @"c:\windows\system32\iisreset.exe";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            using (Process iisresetProcess = Process.Start(info))
            {
                while (!iisresetProcess.WaitForExit(100))
                {
                    if (!iisresetProcess.Responding)
                    {
                        break;
                    }
                }
            }
        }
    }
}
