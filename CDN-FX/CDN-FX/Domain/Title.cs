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

namespace CDN_FX
{

    class Title
    {
        public Title()
        {

        }

        public Title(string Name, string Region, string Serial, string TitleID)
        {
            this.Name = Name;
            this.Region = Region;
            this.Serial = Serial;
            this.TitleID = TitleID;
            download = false;
        }

        public Title(byte[] Data, string TitleID, string ConsoleID, int CommonKeyIndex)
        {
            data = Data;
            this.TitleID = TitleID;
            this.ConsoleID = ConsoleID;
            commonKeyIndex = CommonKeyIndex;
            download = false;
            Name = "";
            Serial = "";
            Region = "";
        }

        public string Name { get; set; }
        public string Region { get; set; }
        public string Serial { get; set; }

        private string titleID;

        public string TitleID {
            get
            {
                return titleID;
            }
            set
            {
                //Set Type
                string typecheck = value.Substring(4,4).ToLower();

                if (typecheck.Equals("0000"))
                    Type = "eShopApp";
                else if (typecheck.Equals("0001"))
                    Type = "DLP";
                else if (typecheck.Equals("0002"))
                    Type = "Demo";
                else if (typecheck.Equals("000e"))
                    Type = "Update";
                else if (typecheck.Equals("008c"))
                    Type = "DLC";
                else if (typecheck.Equals("8004"))
                    Type = "DSiWare";
                else if ((Convert.ToInt32(typecheck, 16) & 0x10) == 0x10)
                    Type = "System";
                else if (typecheck.Equals("8005"))
                    Type = "DSiSysApp";
                else if (typecheck.Equals("800f"))
                    Type = "DSiSysData";
                else
                    Type = "Mystery";

                titleID = value;
            }
        }

        public string ConsoleID { get; set; }
        public string TitleKey { get; set; }
        
        public int commonKeyIndex { get; set; }
        public string Type { get; set; }
        public byte[] data { get; set; }

        public bool download { get; set; }

    }
}
