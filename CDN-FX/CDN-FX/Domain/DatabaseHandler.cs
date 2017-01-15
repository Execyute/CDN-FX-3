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

using CDN_FX;
using IDBE;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CDN_FX.Domain
{
    static class DatabaseHandler
    {

        private static ObservableCollection<Title> mergedDatabase = new ObservableCollection<Title>();

        public static void mergeDatabases()
        {
            if(mergedDatabase.Count == 0)
            {
                string name = "", region = "", serial = "", titleid = "";
                ObservableCollection<Title> tmpDatabase = new ObservableCollection<Title>();

                Log.Information("DBHANDLER: Merging databases...");

                // Read community.xml
                if (File.Exists("databases/community.xml"))
                {
                    Log.Information("DBHANDLER: Reading community.xml");

                    XDocument doc = XDocument.Load("databases/community.xml");
                    var titles3DSDB = doc.Descendants("Ticket");

                    foreach (var titleElement in titles3DSDB)
                    {
                        name = titleElement.Element("name").Value;
                        region = titleElement.Element("region").Value;
                        serial = titleElement.Element("serial").Value;
                        titleid = titleElement.Element("titleid").Value.ToLower();

                        mergedDatabase.Add(new Title(name, region, serial, titleid));
                    }
                }

                // Read 3dsdb.xml
                if (File.Exists("databases/3dsdb.xml"))
                {
                    Log.Information("DBHANDLER: Reading 3dsdb.xml");

                    XDocument doc = XDocument.Load("databases/3dsdb.xml");
                    var titles3DSDB = doc.Descendants("release");

                    foreach (var titleElement in titles3DSDB)
                    {
                        name = titleElement.Element("name").Value;
                        region = titleElement.Element("region").Value;
                        serial = titleElement.Element("serial").Value;
                        titleid = titleElement.Element("titleid").Value.ToLower();

                        if (!mergedDatabase.Any(x => x.TitleID.Equals(titleid)) && titleid.Length == 16)
                        {
                            tmpDatabase.Add(new Title(name, region, serial, titleid));
                        }

                    }
                }

                foreach (Title title in tmpDatabase)
                    mergedDatabase.Add(title);

            }

        }

        public static ObservableCollection<Title> readDatabasesOnly()
        {
            Log.Information("DBHANDLER: Reading databases...");
            mergeDatabases();
            return mergedDatabase;
        }

        public static ObservableCollection<Title> readDatabases(ObservableCollection<Title> titlelist)
        {
            ObservableCollection<Title> newTitleList = new ObservableCollection<Title>();
            ObservableCollection<Title> newTitleList2 = new ObservableCollection<Title>();

            Log.Information("DBHANDLER: Reading databases...");

            // Add all Titles without data
            foreach (Title title in titlelist)
                newTitleList.Add(new Title { TitleID = title.TitleID, ConsoleID = title.ConsoleID, Name = "", Region = "", Serial = "", download = false});

            // Set metadata
            foreach(Title title in newTitleList)
            {
                bool added = false;

                foreach (Title entry in mergedDatabase)
                {
                    if (title.TitleID.ToLower().Equals(entry.TitleID.ToLower()))
                    {
                        added = true;
                        newTitleList2.Add(new Title { Name = entry.Name, Region = entry.Region, Serial = entry.Serial, TitleID = entry.TitleID, ConsoleID = title.ConsoleID, download = false });
                        break;
                    }
                }

                if (!added)
                {
                    newTitleList2.Add(title);
                }
                    
            } 


            newTitleList = newTitleList2;

            return newTitleList;
        }

    }
}
