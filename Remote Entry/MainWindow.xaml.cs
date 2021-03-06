﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

namespace Remote_Entry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread RestartThread { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RestartThread = new Thread(restartEntry);
            RestartThread.Start();
        }

        private void restartEntry()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = @"/F /IM ClientIPScanner.exe";
            info.FileName = @"c:\windows\system32\taskkill.exe";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            using (Process cipKillProcess = Process.Start(info))
            {
                while (!cipKillProcess.WaitForExit(100))
                {
                    if (!cipKillProcess.Responding)
                    {
                        break;
                    }
                }
            }

            info = new ProcessStartInfo();
            info.Arguments = @"/F /IM mstsc.exe";
            info.FileName = @"c:\windows\system32\taskkill.exe";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            using (Process mstscKillProcess = Process.Start(info))
            {
                while (!mstscKillProcess.WaitForExit(100))
                {
                    if (!mstscKillProcess.Responding)
                    {
                        break;
                    }
                }
            }


            bool? internetOkay = false;
            bool firstLoop = true;
            while (internetOkay == false)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        using (client.OpenRead("https://www.google.com/generate_204"))
                        {
                            if (internetOkay == false && !firstLoop)
                            {
                                setStatusText("Internet Connection Restored!");
                                Dispatcher.Invoke(() => { ErrorTextBlock.Visibility = Visibility.Collapsed; Height = 150; });
                                Thread.Sleep(1000);
                            }
                            internetOkay = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    setStatusText("Internet Connection Offline!");
                    internetOkay = false;
                    Dispatcher.Invoke(() => { ErrorTextBlock.Visibility = Visibility.Visible; Height = 250; });
                    Thread.Sleep(1000);
                }
                firstLoop = false;
            }

            DateTime startTime = DateTime.Now;
            DateTime elapsedTime = DateTime.Now;
            while ((elapsedTime - startTime).TotalSeconds < 15)
            {
                elapsedTime = DateTime.Now;
                setStatusText(string.Format("Entry restarting: {0} seconds left", (30 - (int)(elapsedTime - startTime).TotalSeconds)));
                Thread.Sleep(500);
            }

            try
            {
                info = new ProcessStartInfo();
                info.FileName = @"c:\Program Files (x86)\Card Scanning Solutions\IPScan\clientipscanner.exe";
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                using (Process cipStartProcess = Process.Start(info))
                {
                    while (!cipStartProcess.WaitForExit(100))
                    {
                        if (!cipStartProcess.Responding)
                        {
                            break;
                        }
                    }
                }
            } catch (Exception ex)
            {

            }

            startTime = DateTime.Now;
            elapsedTime = DateTime.Now;
            while ((elapsedTime - startTime).TotalSeconds < 15)
            {
                elapsedTime = DateTime.Now;
                setStatusText(string.Format("Entry restarting: {0} seconds left", (15 - (int)(elapsedTime - startTime).TotalSeconds)));
                Thread.Sleep(500);
            }


            FileInfo file = new FileInfo(string.Format("{0}\\SafeHouse.rdp", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
            if (!file.Exists)
            {
                file = new FileInfo(string.Format("{0}\\SafeHouse.rdp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)));
            }

            info = new ProcessStartInfo();
            info.Arguments = file.FullName;
            info.FileName = @"c:\windows\System32\mstsc.exe";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardInput = true;
            using (Process rdpStartProcess = Process.Start(info))
            {
                while (!rdpStartProcess.WaitForExit(100))
                {
                    if (!rdpStartProcess.Responding)
                    {
                        break;
                    }
                }
            }

            try
            {
                Dispatcher.Invoke(() => { Application.Current.Shutdown(); });
            } catch (Exception ex)
            {

            }
        }

        private void setStatusText(string text)
        {
            try
            {
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(() => { setStatusText(text); });
                    return;
                }

                StatusTextBlock.Text = text;
            } catch (Exception ex)
            {

            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (RestartThread != null && RestartThread.IsAlive)
            {
                RestartThread.Abort();
            }
        }
    }
}
