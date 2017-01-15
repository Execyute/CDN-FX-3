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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CDN_FX
{
    class Updater
    {
        private readonly string updateURL = "https://raw.githubusercontent.com/Ptrk25/GroovyFX/gh-pages/programV3/cdn-fx.bin";
        private readonly string updateCheckURL = "https://raw.githubusercontent.com/Ptrk25/GroovyFX/gh-pages/programV3/check.txt";
        private readonly string updateMessageURL = "https://raw.githubusercontent.com/Ptrk25/GroovyFX/gh-pages/programV3/change.log";
        private readonly string CommunityDatabaseUpdateURL = "https://raw.githubusercontent.com/Ptrk25/GroovyFX/gh-pages/database/community.xml";
        private readonly string CommunityDatabaseUpdateCheckURL = "https://raw.githubusercontent.com/Ptrk25/GroovyFX/gh-pages/database/check.txt";
        private readonly string DSDBDatabaseURL = "http://3dsdb.com/xml.php";
        private readonly string FirmwareDatabaseURL = "http://yls8.mtheall.com/ninupdates/titlelist.php?sys=ctr&csv=1";

        private readonly string VERSION = "dev-3.0";


        public Updater()
        {

        }

        // Checks if a CDN-FX update exist
        public bool CheckForUpdateProgram()
        {
            Log.Information("UPDATER: Searching for new program update...");

            string version;
            using (var wc = new WebClient())
            {
                version = wc.DownloadString(updateCheckURL);
            }

            if (!version.Equals(VERSION))
                Log.Information("UPDATER: Update found!");

            Log.Information("UPDATER: No update found!");

            return !version.Equals(VERSION);

        }

        // Return update message of CDN-FX
        public string UpdateMessage()
        {
            string message;
            using (var wc = new WebClient())
            {
                message = wc.DownloadString(updateMessageURL);
            }

            return message;
        }

        // Updates CDN-FX
        public void updateProgram()
        {
            Log.Information("UPDATER: Updating CDN-FX...");

            WebClient wc = new WebClient();

            try
            {
                wc.DownloadFile(updateURL, "CDN-FX.exe");
                Log.Information("UPDATER: Update complete!");
            }
            catch(WebException ex)
            {
                Log.Error("UPDATER: Update failed!", ex.Message);
            }

        }

        // Checks if a community.xml update exist
        public bool CheckForCommunityXMLUpdate()
        {
            Log.Information("UPDATER: Searching for community.xml new update...");

            string version;
            using (var wc = new WebClient())
            {
                version = wc.DownloadString(CommunityDatabaseUpdateCheckURL);
            }

            if (!version.Equals(Properties.Settings.Default.CommunityXMLVersion))
            {
                Properties.Settings.Default.CommunityXMLVersion = version;
                Log.Information("UPDATER: Update found!");
                return true;
            }

            if (!File.Exists("databases/community.xml"))
            {
                Log.Information("UPDATER: Update found!");
                return true;
            }

            Log.Information("UPDATER: No update found!");
            return false;    
        }

        // Updates community.xml
        public void updateCommunityDatabase()
        {
            Log.Information("UPDATER: Updating community.xml...");

            Properties.Settings.Default.Save();

            WebClient wc = new WebClient();

            try
            {
                wc.DownloadFile(CommunityDatabaseUpdateURL, "databases/community.xml");
                Log.Information("UPDATER: Update complete!");
            }
            catch (WebException ex)
            {
                Log.Error("UPDATER: Update failed!", ex.Message);
            }
        }

        // Updates 3dsdb.xml
        public void update3DSDBDatabase()
        {
            Log.Information("UPDATER: Updating 3dsdb.xml...");

            Properties.Settings.Default.Save();

            WebClient wc = new WebClient();

            try
            {
                wc.DownloadFile(DSDBDatabaseURL, "databases/3dsdb.xml");
                Log.Information("UPDATER: Update complete!");
            }
            catch (WebException ex)
            {
                Log.Error("UPDATER: Update failed!", ex.Message);
            }
        }

        // Updates titlelist.csv
        public void updateFirmwareDatabase()
        {
            Log.Information("UPDATER: Updating titlelist.csv...");

            Properties.Settings.Default.Save();

            WebClient wc = new WebClient();

            try
            {
                wc.DownloadFile(FirmwareDatabaseURL, "databases/titlelist.csv");
                Log.Information("UPDATER: Update complete!");
                
            }
            catch (WebException ex)
            {
                Log.Error("UPDATER: Update failed!", ex.Message);
            }
        }

    }
}
