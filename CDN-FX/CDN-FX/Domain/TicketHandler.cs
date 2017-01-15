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
    class TicketHandler
    {

        private readonly byte[] TICKET_HEADER = ConvertingTools.HexStringToByteArray("526f6f742d434130303030303030332d58533030303030303063");
        private byte[] fileData = null;
        private readonly int tk = 0x140;

        public ObservableCollection<Title> ticketlist { get; set; }
        public List<int> ticketCount { get; set; }

        public TicketHandler()
        {
            ticketlist = new ObservableCollection<Title>();
            ticketCount = new List<int>();
        }

        public void openFile(string path)
        {
            Log.Information("THANDLER: Reading ticket.db...");
            fileData = File.ReadAllBytes(path);
        }

        public bool readTicketFile()
        {
            int tmp = 0, commonKeyIndex = 0;
            List<int> ticketOffsets = new List<int>();

            while(tmp != -1)
            {
                tmp = ConvertingTools.IndexOf(fileData, TICKET_HEADER, tmp+1);
                if (tmp != -1)
                    ticketOffsets.Add(tmp);
            }

            if (ticketOffsets.Count == 0)
                return false;

            foreach(int offs in ticketOffsets)
            {
                commonKeyIndex = fileData[offs + 0xB1];

                if (fileData[offs + 0x7C] != 0x1)
                    continue;

                if (commonKeyIndex > 5)
                    continue;

                byte[] ticketData = ConvertingTools.copyOfRange(fileData, offs-0x140, offs+0x210);
                byte[] titleID = ConvertingTools.copyOfRange(ticketData, tk+0x9C, tk+0xA4);
                byte[] consoleID = ConvertingTools.copyOfRange(ticketData, tk + 0x98, tk + 0x9C);
                string titleid, consoleid;
                int cki = fileData[offs + 0xB1];

                titleid = ConvertingTools.ByteArrayToString(titleID);
                consoleid = ConvertingTools.ByteArrayToString(consoleID);

                ticketlist.Add(new Title(ticketData, titleid, consoleid, cki));
            }

            return true;
        }

        public void countTickets()
        {
            List<string> counter = new List<string>();
            List<Title> not_eshop_tickets = new List<Title>();
            List<Title> eshop_tickets = new List<Title>();
            int systik = 0;

            ticketCount.Add(ticketlist.Count);

            foreach(Title tik in ticketlist)
                counter.Add(tik.TitleID);

            ticketCount.Add(ConvertingTools.removeDuplicates(counter).Count);
            ticketCount.Add(counter.Count - ticketCount[1]);

            counter.Clear();

            foreach(Title tik in ticketlist)
            {
                if (tik.Type.Equals("System") || tik.Type.Equals("DSiSysApp") || tik.Type.Equals("DSiSysData"))
                    systik++;
            }

            ticketCount.Add(systik);

            foreach(Title tik in ticketlist)
            {
                if (tik.commonKeyIndex == 0)
                    if (tik.ConsoleID.Equals("00000000"))
                        not_eshop_tickets.Add(tik);
                    else
                    {
                        eshop_tickets.Add(tik);
                        counter.Add(tik.TitleID);
                    }    
            }

            ticketCount.Add(eshop_tickets.Count);
            ticketCount.Add(ConvertingTools.removeDuplicates(counter).Count);
            ticketCount.Add(eshop_tickets.Count - ticketCount[5]);
            ticketCount.Add(not_eshop_tickets.Count);

            removeDuplicateTickets();
        }

        private void removeDuplicateTickets()
        {

            ObservableCollection<Title> newTitleList = new ObservableCollection<Title>();

            foreach(Title tik in ticketlist)
            {
                string titleid = tik.TitleID, consoleid = tik.ConsoleID;

                if (!ConvertingTools.ByteArrayToString(ConvertingTools.copyOfRange(tik.data, 0x00, 0x04)).Contains("00010004"))
                    continue;

                if (newTitleList.Count > 0)
                {
                    foreach(Title tik2 in newTitleList)
                    {
                        string titleid2 = tik2.TitleID, consoleid2 = tik2.ConsoleID;
                        if (titleid2.Equals(titleid) && consoleid2.Equals(consoleid))
                        {
                            tik2.data = tik.data;

                            goto outerloop;
                        }
                    }
                    newTitleList.Add(tik);
                }
                else
                    newTitleList.Add(tik);
                outerloop:;

            }
            ticketlist = newTitleList;
        }

    }
}
