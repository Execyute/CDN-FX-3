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
using System.Windows;
using System.Windows.Forms;

namespace CDN_FX.UserControls.Settings
{
    /// <summary>
    /// Interaktionslogik für TicketManager.xaml
    /// </summary>
    public partial class TicketManager : System.Windows.Controls.UserControl
    {
        public TicketManager()
        {
            InitializeComponent();
        }

        private void btnSelectTicketDB_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "ticket.db files (*.db)|*.db|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;

            if(fileDialog.ShowDialog() == DialogResult.OK)
                tbTicketDB.Text = fileDialog.FileName;
        }

        private void btnSelectOutput_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            if (folderDialog.ShowDialog() == DialogResult.OK)
                tbDefaultOutput.Text = folderDialog.SelectedPath;
        }
    }
}
