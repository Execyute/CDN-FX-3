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

using CDN_FX.Domain;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace CDN_FX
{
    /// <summary>
    /// Interaktionslogik für TitleDLManual.xaml
    /// </summary>
    public partial class TitleDLAuto : UserControl
    {

        private ObservableCollection<Title> orginalList;
        private ObservableCollection<Title> orginalTitleList;
        private List<int> counts;

        public TitleDLAuto()
        {
            InitializeComponent();
        }

        public void loadEncTitleKeyFile()
        {
            EncTitleKeysHandler ehandler = new EncTitleKeysHandler();

            ehandler.openFile(Properties.Settings.Default.CurrentTDLEncTitleKeyPath);
            ehandler.readEncTitleKeyFile();
            ehandler.countTitles();

            DatabaseHandler.mergeDatabases();

            orginalTitleList = ehandler.titlelist;
            orginalList = DatabaseHandler.readDatabases(ehandler.titlelist);
            counts = ehandler.apptypeCount;
        }

        public void initTable()
        {
            var oTitleList = orginalList;

            var itemSourceList = new CollectionViewSource() { Source = oTitleList };
            itemSourceList.Filter += new FilterEventHandler(Filter);

            ICollectionView ItemList = itemSourceList.View;

            ucTable.dgTable.ItemsSource = ItemList;
            orginalList = oTitleList;
        }

        public void updateCounters()
        {
            ucEntries.lblTotalCount.Text = orginalTitleList.Count.ToString();
            ucEntries.lbleShopCount.Text = counts[0].ToString();
            ucEntries.lblDLPCount.Text = counts[1].ToString();
            ucEntries.lblDemoCount.Text = counts[2].ToString();
            ucEntries.lblUpdateCount.Text = counts[3].ToString();
            ucEntries.lblDLCCount.Text = counts[4].ToString();
            ucEntries.lblDSiWareCount.Text = counts[5].ToString();
            ucEntries.lblDSiSysAppsCount.Text = counts[7].ToString();
            ucEntries.lblDSiSysDataCount.Text = counts[8].ToString();
            ucEntries.lblSystemCount.Text = counts[6].ToString();
        }

        private void Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = true;
        }

        public void filterTable()
        {

            var itemSourceList = new CollectionViewSource() { Source = orginalList };
            itemSourceList.Filter += new FilterEventHandler(tableFilter);

            ICollectionView ItemList = itemSourceList.View;

            ucTable.dgTable.ItemsSource = ItemList;
        }

        // Filterlogic
        private void tableFilter(object sender, FilterEventArgs e)
        {
            TextBlock tbCategory = (TextBlock)ucCategory.lbCatList.SelectedItem;

            var title = e.Item as Title;
            string category = tbCategory.Text.ToLower();
            string searchText = ucTable.tbSearch.Text.ToLower();

            string name = title.Name.ToLower();
            string region = title.Region.ToLower();
            string serial = title.Serial.ToLower();
            string type = title.Type.ToLower();

            if (category.Equals("all"))
                category = "";

            if (name.Contains(searchText) || title.TitleID.Contains(searchText) || serial.Contains(searchText) || type.Contains(searchText) || region.Contains(searchText))
            {
                if (category.Length > 0)
                {
                    if (type.Equals(category))
                        e.Accepted = true;
                    else
                        e.Accepted = false;
                }
                else
                {
                    e.Accepted = true;
                }
            }
            else
                e.Accepted = false;
        }

    }
}
