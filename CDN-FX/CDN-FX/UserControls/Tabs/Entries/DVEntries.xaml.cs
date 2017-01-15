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
using System;
using System.Collections.Generic;
using System.IO;
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

namespace CDN_FX.UserControls.Entries
{
    /// <summary>
    /// Interaktionslogik für DVEntries.xaml
    /// </summary>
    public partial class DVEntries : UserControl
    {
        public DVEntries()
        {
            InitializeComponent();
            if (File.Exists("databases/community.xml") || File.Exists("databases/3dsdb.xml") || File.Exists("databases/idbe.xml"))
                initTitleCounter();
        }

        public void initTitleCounter()
        {
            var oTitleList = DatabaseHandler.readDatabasesOnly();
            int num = 0;

            foreach (Title title in oTitleList)
            {
                switch (title.Type)
                {
                    case "eShopApp":
                        num = Convert.ToInt32(lbleShopCount.Text);
                        num++;
                        lbleShopCount.Text = num.ToString();
                        break;
                    case "DLP":
                        num = Convert.ToInt32(lblDLPCount.Text);
                        num++;
                        lblDLPCount.Text = num.ToString();
                        break;
                    case "Demo":
                        num = Convert.ToInt32(lblDemoCount.Text);
                        num++;
                        lblDemoCount.Text = num.ToString();
                        break;
                    case "Update":
                        num = Convert.ToInt32(lblUpdateCount.Text);
                        num++;
                        lblUpdateCount.Text = num.ToString();
                        break;
                    case "DLC":
                        num = Convert.ToInt32(lblDLCCount.Text);
                        num++;
                        lblDLCCount.Text = num.ToString();
                        break;
                    case "DSiWare":
                        num = Convert.ToInt32(lblDSiWareCount.Text);
                        num++;
                        lblDSiWareCount.Text = num.ToString();
                        break;
                    case "DSiSysApp":
                        num = Convert.ToInt32(lblDSiSysAppsCount.Text);
                        num++;
                        lblDSiSysAppsCount.Text = num.ToString();
                        break;
                    case "DSiSysData":
                        num = Convert.ToInt32(lblDSiSysDataCount.Text);
                        num++;
                        lblDSiSysDataCount.Text = num.ToString();
                        break;
                    case "System":
                        num = Convert.ToInt32(lblSystemCount.Text);
                        num++;
                        lblSystemCount.Text = num.ToString();
                        break;
                    default:
                        break;
                }

                lblTotalEntriesCount.Text = oTitleList.Count.ToString();

            }
        }
    }
}
