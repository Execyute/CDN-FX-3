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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CDN_FX.UserControls
{
    /// <summary>
    /// Interaktionslogik für AppCategory.xaml
    /// </summary>
    public partial class AppCategory : UserControl
    {
        public AppCategory()
        {
            InitializeComponent();
        }

        private bool changed = false;

        private void lbCatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (changed)
            {
                DependencyObject ucParent = this.Parent;

                while (!(ucParent is UserControl))
                {
                    ucParent = LogicalTreeHelper.GetParent(ucParent);
                }

                // TitleDatabaseViewer
                if (ucParent.GetType().Equals(typeof(TitleDatabaseViewer))){
                    TitleDatabaseViewer tabTDV = (TitleDatabaseViewer)ucParent;
                    tabTDV.ucTable.filterTable();
                }

                // Ticket Manager
                if (ucParent.GetType().Equals(typeof(TicketManager)))
                {
                    TicketManager tabTM = (TicketManager)ucParent;
                    tabTM.filterTable();
                }

                // Title Downloader Auto
                if (ucParent.GetType().Equals(typeof(TitleDLAuto)))
                {
                    TitleDLAuto tabTDL = (TitleDLAuto)ucParent;
                    tabTDL.filterTable();
                }

            }
            else
                changed = true;
            
        }
    }
}
