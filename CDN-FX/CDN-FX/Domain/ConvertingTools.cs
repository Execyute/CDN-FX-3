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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDN_FX.Domain
{
    class ConvertingTools
    {

        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();
        }

        public static int IndexOf(byte[] arrayToSearchThrough, byte[] patternToFind, int pos)
        {
            if (patternToFind.Length > arrayToSearchThrough.Length)
                return -1;

            if (pos > arrayToSearchThrough.Length)
                return -1;

            for (int i = pos; i < arrayToSearchThrough.Length - patternToFind.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < patternToFind.Length; j++)
                {
                    if (arrayToSearchThrough[i + j] != patternToFind[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }

        public static byte[] copyOfRange(byte[] source, int startPos, int endPos)
        {
            byte[] end = new byte[(endPos - startPos)];

            Array.Copy(source, startPos, end, 0, (endPos - startPos));

            return end;
        }

        public static byte[] connectByteArray(byte[] b1, byte[] b2, byte[] b3)
        {
            byte[] add = new byte[b1.Length + b2.Length + b3.Length];

            Array.Copy(b1, 0, add, 0, b1.Length);
            Array.Copy(b2, 0, add, b1.Length, b2.Length);
            Array.Copy(b3, 0, add, b1.Length + b2.Length, b3.Length);

            return add;
        }

        public static List<string> removeDuplicates(List<string> list)
        {
            // Store unique items in result
            List<string> result = new List<string>();

            // Record encountered String in HashSet
            HashSet<string> set = new HashSet<string>();

            // Loop over argument list
            foreach(string item in list)
            {
                // If String is not is set, add it to the list and the set
                if (!set.Contains(item))
                {
                    result.Add(item);
                    set.Add(item);
                }
            }
            return result;
        }

    }
}
