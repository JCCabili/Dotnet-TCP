using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.LAN
{
    public enum eState
    {
       OutofService =  0x00,
       InService = 0x01,
       Offline = 0x02,
       Online = 0x03,
       Maintenance = 0x04,
       Invalid = 0x05

    }
}
