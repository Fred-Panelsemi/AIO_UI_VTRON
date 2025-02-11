using System;
using System.Collections.Generic;
using System.Text;

namespace AIO.EquipmentIP
{
    public class UC_EquipmentIPEventArgs:EventArgs
    {
        public string IPAddr { set; get; }
        public string SubnetMask { set; get; }
        public string DefaultGateway { set; get; }
    }
}
