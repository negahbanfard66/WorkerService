using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConsumer_Project.Core
{
    public class LogRequestParameters
    {
        public string RequestID { get; set; }

        public string Type { get; set; }

        public string Level { get; set; }

        public string DateTime { get; set; }

        public string MachineId { get; set; }

        public string SessionId { get; set; }
    }
}
