using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models
{
    public class QueryDailyYield
    {
        public string Plant { get; set; }
        public string Cust2Code { get; set; }
        public string Cust3Code { get; set; }
        public string PKGCode { get; set; }
        public string StageCode { get; set; }
        public string DeviceName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }



    }
}
