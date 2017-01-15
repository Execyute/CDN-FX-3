/*
    Copyright (C) 2016 Ptrk25

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Serilog;
using System.IO;
using System.Windows.Forms;

namespace CDN_FX.Windows
{
    /// <summary>
    /// Interaktionslogik für Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();

            initConfig();
            initLogger();
            initFolder();
        }

        // Creates config file if doesn't exists
        private void initConfig()
        {
            if (!File.Exists("CDN-FX.exe.config"))
            {
                File.WriteAllText(@"CDN-FX.exe.config", Properties.Resources.CDN_FX_exe);
            }
        }

        private void initFolder()
        {
            if (!Directory.Exists("databases"))
                Directory.CreateDirectory("databases");
        }

        // Init DebugLogger
        private void initLogger()
        {
            if (Properties.Settings.Default.DebugMode)
            {
                if (File.Exists("debuglog.txt"))
                    File.Delete("debuglog.txt");
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File("debuglog.txt")
                    .CreateLogger();
            }

            Log.Information("SPLASH: Logger initialized!");
        }

        private async void Win_ContentRendered(object sender, EventArgs e)
        {
            await Task.Run(() => initProgram());

            MainWindow main = new MainWindow();

            await Task.Run(() => initTM());
            if(Properties.Settings.Default.DefaultTMTicketDBPath.Length > 1)
            {
                Properties.Settings.Default.CurrentTMTicketDBPath = Properties.Settings.Default.DefaultTMTicketDBPath;
                main.ucTicketManager.loadTicketDBFile();
                main.ucTicketManager.initTable();
                main.ucTicketManager.updateCounters();
                Log.Information("SPLASH: Ticket.db loaded!");
            }

            await Task.Run(() => initTDL());
            if (Properties.Settings.Default.DefaultTDLEncTitleKeyPath.Length > 1)
            {
                Properties.Settings.Default.CurrentTDLEncTitleKeyPath = Properties.Settings.Default.DefaultTDLEncTitleKeyPath;
                main.ucTitleDownloaderAuto.loadEncTitleKeyFile();
                main.ucTitleDownloaderAuto.initTable();
                main.ucTitleDownloaderAuto.updateCounters();
                Log.Information("SPLASH: EncTitleKey.bin loaded!");
            }

            await Task.Run(() => initDV());
            if (File.Exists("databases/community.xml") || File.Exists("databases/3dsdb.xml") || File.Exists("databases/idbe.xml"))
            {
                main.ucTitledatabaseViewer.ucTable.initTable();
                Log.Information("SPLASH: Database Viewer loaded!");
            }
                

            await Task.Run(() => initDone());

            Log.Information("SPLASH: Initialization complete!");

            // Close Splashscreen
            main.Show();
            Close();
        }

        // Handels progress statusS
        private void initProgram()
        {
            Updater updater = new Updater();
            
            // Stage 1 Program update
            Dispatcher.Invoke(() =>
            {
                spTLabel.Text = "Search for update...";
                spPBProgress.Value = 10;
            });

            if (updater.CheckForUpdateProgram())
            {
                Dispatcher.Invoke(() =>
                {
                    spTLabel.Text = "Update found!";
                });

                // Ask for update
                if (askForUpdate())
                {
                    Dispatcher.Invoke(() =>
                    {
                        spTLabel.Text = "Updating...";
                        spPBProgress.Value = 20;
                    });
                    updater.updateProgram();
                }
            }

            // Stage 2 CommunityXML update
            Dispatcher.Invoke(() =>
            {
                spTLabel.Text = "Search for community.xml update...";
                spPBProgress.Value = 30;
            });

            if (Properties.Settings.Default.AutoUpdateCommunityXML)
            {
                if (updater.CheckForCommunityXMLUpdate())
                {
                    Dispatcher.Invoke(() =>
                    {
                        spTLabel.Text = "Updating community.xml...";
                        spPBProgress.Value = 40;
                    });
                    updater.updateCommunityDatabase();
                }
            }

            // Stage 3 3DSDBXML update
            if (Properties.Settings.Default.AutoUpdate3DSDBXML)
            {
                Dispatcher.Invoke(() =>
                {
                    spTLabel.Text = "Updating 3dsdb.xml...";
                    spPBProgress.Value = 50;
                });
                updater.update3DSDBDatabase();
            }

            // Stage 4 titlelist.csv update
            if (Properties.Settings.Default.AutoUpdateTitlelistCSV)
            {
                Dispatcher.Invoke(() =>
                {
                    spTLabel.Text = "Updating titlelist.csv...";
                    spPBProgress.Value = 60;
                });
                updater.updateFirmwareDatabase();
            }

        }

        private void initDV()
        {
            Dispatcher.Invoke(() =>
            {
                spTLabel.Text = "Reading databases...";
                spPBProgress.Value = 90;
            });
            Thread.Sleep(10);
        }

        private void initTM()
        {
            Dispatcher.Invoke(() =>
            {
                string[] filenames = Properties.Settings.Default.DefaultTMTicketDBPath.Split('\\');
                spTLabel.Text = "Reading " + filenames[filenames.Length-1] + "...";
                spPBProgress.Value = 70;
            });
            Thread.Sleep(50);
        }

        private void initTDL()
        {
            Dispatcher.Invoke(() =>
            {
                string[] filenames = Properties.Settings.Default.DefaultTDLEncTitleKeyPath.Split('\\');
                spTLabel.Text = "Reading " + filenames[filenames.Length - 1] + "...";
                spPBProgress.Value = 80;
            });
            Thread.Sleep(50);
        }

        private void initDone()
        {
            Dispatcher.Invoke(() =>
            {
                spTLabel.Text = "Starting...";
                spPBProgress.Value = 100;
            });

            Thread.Sleep(500);
        }

        private bool askForUpdate()
        {
            return false;
        }


    }
}
