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
using System.Text;
using System.Threading.Tasks;

namespace CDN_FX.Domain
{
    class EncTitleKeysHandler
    {

        private readonly byte[] TICKETTEMPLATE = ConvertingTools.HexStringToByteArray("00010004d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0d15ea5e0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000526f6f742d434130303030303030332d585330303030303030630000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000cccccccccccccccccccccccccccccccc00000000000000000000000000aaaaaaaaaaaaaaaa00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010014000000ac000000140001001400000000000000280000000100000084000000840003000000000000ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
        private readonly int TICKETLEN = 32;
        private readonly int TK = 0x140;
        private byte[] fileData = null;

        public ObservableCollection<Title> titlelist { get; set; }
        public List<int> apptypeCount { get; set; }

        public EncTitleKeysHandler()
        {
            titlelist = new ObservableCollection<Title>();
            apptypeCount = new List<int>(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
        }

        public void openFile(string path)
        {
            Log.Information("THANDLER: Reading encTitleKeys.bin...");
            fileData = File.ReadAllBytes(path);
        }

        public void readEncTitleKeyFile()
        {
            long datalen = fileData.Length;
            byte[] titleid = new byte[0x8];
            byte[] titlekey = new byte[0x16];

            for(long i = 16; i < datalen; i += TICKETLEN)
            {
                Title title = new Title();
                title.Name = "";
                title.Region = "";
                title.Serial = "";

                Array.Copy(fileData, i+8, titleid, 0, 8);
                Array.Copy(fileData, i+16, titlekey, 0, 16);

                title.TitleID = ConvertingTools.ByteArrayToString(titleid);
                title.TitleKey = ConvertingTools.ByteArrayToString(titlekey);
                title.data = createTicketData(title.TitleID, title.TitleKey);

                titlelist.Add(title);
            }

        }

        private byte[] createTicketData(string TitleID, string TitleKey)
        {
            byte[] ticket = TICKETTEMPLATE;

            ticket = ConvertingTools.connectByteArray(ConvertingTools.copyOfRange(ticket, 0, TK + 0x9C), ConvertingTools.HexStringToByteArray(TitleID), ConvertingTools.copyOfRange(ticket, TK + 0xA4, ticket.Length));
            ticket = ConvertingTools.connectByteArray(ConvertingTools.copyOfRange(ticket, 0, TK + 0x7F), ConvertingTools.HexStringToByteArray(TitleKey), ConvertingTools.copyOfRange(ticket, TK + 0x8F, ticket.Length));

            return ticket;
        }

        public void countTitles()
        {
            foreach(Title title in titlelist)
            {
                string type = title.Type;

                if (type.Equals("eShopApp"))
                    apptypeCount[0]++;
                else if (type.Equals("DLP"))
                    apptypeCount[1]++;
                else if (type.Equals("Demo"))
                    apptypeCount[2]++;
                else if (type.Equals("Update"))
                    apptypeCount[3]++;
                else if (type.Equals("DLC"))
                    apptypeCount[4]++;
                else if (type.Equals("DSiWare"))
                    apptypeCount[5]++;
                else if (type.Equals("System"))
                    apptypeCount[6]++;
                else if (type.Equals("DSiSysApp"))
                    apptypeCount[7]++;
                else if (type.Equals("DSiSysData"))
                    apptypeCount[8]++;
            }
        }

    }
}
