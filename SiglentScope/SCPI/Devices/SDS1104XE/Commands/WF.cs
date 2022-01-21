using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SCPI.Devices.SDS1104XE.Commands
{
    public class WF : IQuery
    {
        public Status Status { get; set; }
        public string Channel { get; set; } = "C1";
        public string Query => $"{Channel}:WF? DAT2";

        public byte[] Data { get; set; }

        public bool Parse(byte[] data)
        {
            string header = Encoding.ASCII.GetString(data, 0, 25);
            int ind = header.IndexOf(',');
            int strsize = header[ind+2] - '0';
            string blkSizeStr = header.Substring(ind + 3, strsize);
            if(int.TryParse(blkSizeStr, out int blkSize))
            {
                int start = ind + 3 + blkSizeStr.Length;
                Data = new byte[blkSize];
                Array.Copy(data, start, Data, 0, blkSize);
                return true;
            }
            return false;
        }
    }

}


