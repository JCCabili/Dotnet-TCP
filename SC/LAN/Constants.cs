using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.LAN
{
    public static class Constants
    {
        public static byte[] OUTOFSERVICE = { 0x7F, 0x1 };
        public static byte[] INSERVICE = { 0x7F, 0x2 };
        public static byte[] OFFLINE = { 0x7F, 0x3 };

        public static byte[] ONLINE = { 0x7F, 0x04 };
        public static byte[] MAINTENANCE = { 0x7F, 0x05 };


        public static byte[] RECEIVE = { 0x7F, 0xa };
    }
}
