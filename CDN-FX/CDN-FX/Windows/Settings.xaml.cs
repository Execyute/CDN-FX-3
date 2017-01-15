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

using Serilog;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace CDN_FX.Windows
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        // Loads config file
        private void Window_Loaded(object sender, EventArgs e)
        {
            // General tab
            ucGeneral.cbNameFormat.IsChecked = Properties.Settings.Default.UseCIAStringFormat;
            ucGeneral.cbFolders.IsChecked = !Properties.Settings.Default.UseIndividualFolders;
            ucGeneral.cbDebugMode.IsChecked = Properties.Settings.Default.DebugMode;
            ucGeneral.cbDisableUpdateCommunityXML.IsChecked = !Properties.Settings.Default.AutoUpdateCommunityXML;
            ucGeneral.cbDisableUpdate3DSDBXML.IsChecked = !Properties.Settings.Default.AutoUpdate3DSDBXML;
            ucGeneral.cbDisableUpdateTitlelistCSV.IsChecked = !Properties.Settings.Default.AutoUpdateTitlelistCSV;

            // Ticket Manager tab
            if (Properties.Settings.Default.DefaultTMTicketDBPath.Length > 0)
            {
                ucTicketManager.chbxDefaultTicket.IsChecked = true;
                ucTicketManager.tbTicketDB.Text = Properties.Settings.Default.DefaultTMTicketDBPath;
            }
            if(Properties.Settings.Default.DefaultTMOutputPath.Length > 0)
            {
                ucTicketManager.chbxDefaultOutput.IsChecked = true;
                ucTicketManager.tbDefaultOutput.Text = Properties.Settings.Default.DefaultTMOutputPath;
            }
            ucTicketManager.cbAllowDLSystemTitles.IsChecked = Properties.Settings.Default.DownloadSystemTitles;
            ucTicketManager.cbAllowDLNonUniqueTitles.IsChecked = Properties.Settings.Default.DownloadNonUniqueTitles;

            // Title Downloader
            if (Properties.Settings.Default.DefaultTDLEncTitleKeyPath.Length > 0)
            {
                ucTitleDownloader.chbxDefaultEnc.IsChecked = true;
                ucTitleDownloader.tbDefaultEncTitleKey.Text = Properties.Settings.Default.DefaultTDLEncTitleKeyPath;
            }
            if (Properties.Settings.Default.DefaultTDLOutputPath.Length > 0)
            {
                ucTitleDownloader.chbxDefaultTDOutput.IsChecked = true;
                ucTitleDownloader.tbDefaultOutput.Text = Properties.Settings.Default.DefaultTDLOutputPath;
            }
            if (Properties.Settings.Default.DefaultTDLTikOutputPath.Length > 0)
            {
                ucTitleDownloader.chbxDefaultTikOutput.IsChecked = true;
                ucTitleDownloader.tbDefaultTikOutput.Text = Properties.Settings.Default.DefaultTDLTikOutputPath;
            }

            // NUS Downloader
            if(Properties.Settings.Default.DefaultNUSOutputPath.Length > 0)
            {
                ucNUSDownloader.chbxDefaultNUSOutput.IsChecked = true;
                ucNUSDownloader.tbDefaultOutput.Text = Properties.Settings.Default.DefaultNUSOutputPath;
            }

        }

        // Saves settings to config file
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            // General tab
            Properties.Settings.Default.UseCIAStringFormat = (bool)ucGeneral.cbNameFormat.IsChecked;
            Properties.Settings.Default.UseIndividualFolders = (bool)!ucGeneral.cbFolders.IsChecked;
            Properties.Settings.Default.DebugMode = (bool)ucGeneral.cbDebugMode.IsChecked;
            Properties.Settings.Default.AutoUpdateCommunityXML = (bool)!ucGeneral.cbDisableUpdateCommunityXML.IsChecked;
            Properties.Settings.Default.AutoUpdate3DSDBXML = (bool)!ucGeneral.cbDisableUpdate3DSDBXML.IsChecked;
            Properties.Settings.Default.AutoUpdateTitlelistCSV = (bool)!ucGeneral.cbDisableUpdateTitlelistCSV.IsChecked;

            // Ticket Manager tab
            if ((bool)ucTicketManager.chbxDefaultTicket.IsChecked)
                Properties.Settings.Default.DefaultTMTicketDBPath = ucTicketManager.tbTicketDB.Text;
            else
                Properties.Settings.Default.DefaultTMTicketDBPath = "";
            if ((bool)ucTicketManager.chbxDefaultOutput.IsChecked)
                Properties.Settings.Default.DefaultTMOutputPath = ucTicketManager.tbDefaultOutput.Text;
            else
                Properties.Settings.Default.DefaultTMOutputPath = "";
            Properties.Settings.Default.DownloadSystemTitles = (bool)ucTicketManager.cbAllowDLSystemTitles.IsChecked;
            Properties.Settings.Default.DownloadNonUniqueTitles = (bool)ucTicketManager.cbAllowDLNonUniqueTitles.IsChecked;

            // Title Downloader
            if ((bool)ucTitleDownloader.chbxDefaultEnc.IsChecked)
                Properties.Settings.Default.DefaultTDLEncTitleKeyPath = ucTitleDownloader.tbDefaultEncTitleKey.Text;
            else
                Properties.Settings.Default.DefaultTDLEncTitleKeyPath = "";
            if ((bool)ucTitleDownloader.chbxDefaultTDOutput.IsChecked)
                Properties.Settings.Default.DefaultTDLOutputPath = ucTitleDownloader.tbDefaultOutput.Text;
            else
                Properties.Settings.Default.DefaultTDLOutputPath = "";
            if ((bool)ucTitleDownloader.chbxDefaultTDOutput.IsChecked)
                Properties.Settings.Default.DefaultTDLTikOutputPath = ucTitleDownloader.tbDefaultTikOutput.Text;
            else
                Properties.Settings.Default.DefaultTDLTikOutputPath = "";

            // NUS Downloader
            if ((bool)ucNUSDownloader.chbxDefaultNUSOutput.IsChecked)
                Properties.Settings.Default.DefaultNUSOutputPath = ucNUSDownloader.tbDefaultOutput.Text;
            else
                Properties.Settings.Default.DefaultNUSOutputPath = "";

            // Write config to file
            Properties.Settings.Default.Save();
            Log.Information("SETTINGS: Settings saved!");
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
