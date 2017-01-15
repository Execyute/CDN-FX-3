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

using CDN_FX.Windows;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace CDN_FX
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            initProgram();
        }

        private void initProgram()
        {
            // Load Paths from Settings
            Properties.Settings.Default.CurrentTMTicketDBPath = Properties.Settings.Default.DefaultTMTicketDBPath;
            Properties.Settings.Default.CurrentTMOutputPath = Properties.Settings.Default.DefaultTMOutputPath;
            Properties.Settings.Default.CurrentTDLEncTitleKeyPath = Properties.Settings.Default.DefaultTDLEncTitleKeyPath;
            Properties.Settings.Default.CurrentTDLOutputPath = Properties.Settings.Default.DefaultTDLOutputPath;
            Properties.Settings.Default.CurrentTDLTikOutputPath = Properties.Settings.Default.DefaultTDLTikOutputPath;
            Properties.Settings.Default.CurrentNUSOutputPath = Properties.Settings.Default.DefaultNUSOutputPath;
        }

        // Menu Settings Button
        private void menuSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.Owner = GetWindow(this);
            settings.ShowDialog();
        }

        // Menu Close Window
        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Menu Open GitHub page
        private void menuGitHub_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Ptrk25/CDN-FX-3");
        }

        // Menu Open GBATemp Thread
        private void menuGBAThread_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://gbatemp.net/threads/release-cdn-fx-the-ultimate-eshop-content-downloader.414004/");
        }

        // Menu Ticket Manager open TicketDB file
        private async void menuTMOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "ticket.db files (*.db)|*.db|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.CurrentTMTicketDBPath = fileDialog.FileName;

                tbProgressIndicator.Text = "Reading " + fileDialog.SafeFileName + "...";
                pbProgress.IsIndeterminate = true;
                menuTMOpen.IsEnabled = false;
                menuTDOpen.IsEnabled = false;

                await Task.Run(() => ucTicketManager.loadTicketDBFile());
                ucTicketManager.initTable();
                ucTicketManager.updateCounters();

                tbProgressIndicator.Text = "";
                pbProgress.IsIndeterminate = false;
                menuTMOpen.IsEnabled = true;
                menuTDOpen.IsEnabled =  true;
            }

        }

        // Menu Select TM Output
        private void menuTMOutput_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Properties.Settings.Default.CurrentTMOutputPath = folderDialog.SelectedPath;
        }

        // Menu TD open encTitleKey.bin
        private async void menuTDOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "encTitleKey.bin files (*.bin)|*.bin|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Properties.Settings.Default.CurrentTDLEncTitleKeyPath = fileDialog.FileName;

                tbProgressIndicator.Text = "Reading " + fileDialog.SafeFileName + "...";
                pbProgress.IsIndeterminate = true;
                menuTMOpen.IsEnabled = false;
                menuTDOpen.IsEnabled = false;

                await Task.Run(() => ucTitleDownloaderAuto.loadEncTitleKeyFile());
                ucTitleDownloaderAuto.initTable();
                ucTitleDownloaderAuto.updateCounters();

            tbProgressIndicator.Text = "";
                pbProgress.IsIndeterminate = false;
                menuTMOpen.IsEnabled = true;
                menuTDOpen.IsEnabled = true;
        }

        // Menu TD Select Output
        private void menuTDOutput_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Properties.Settings.Default.CurrentTDLOutputPath = folderDialog.SelectedPath;
        }

        // Menu TD Select Tik Output
        private void menuTDTikOutput_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Properties.Settings.Default.CurrentTDLTikOutputPath = folderDialog.SelectedPath;
        }

        // Menu TD Select NUS Output
        private void menuNUSOutput_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Properties.Settings.Default.CurrentNUSOutputPath = folderDialog.SelectedPath;
        }

        // Clear Current Properties
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.CurrentTMTicketDBPath = "";
            Properties.Settings.Default.CurrentTMOutputPath = "";
            Properties.Settings.Default.CurrentTDLEncTitleKeyPath = "";
            Properties.Settings.Default.CurrentTDLOutputPath = "";
            Properties.Settings.Default.CurrentTDLTikOutputPath = "";
            Properties.Settings.Default.CurrentNUSOutputPath = "";
        }
    }
}
