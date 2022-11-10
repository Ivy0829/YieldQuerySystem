using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models
{
    public class CloseYieldModel
    {
        public string Seq {get;set;}
        public string Fac { get;set;}
        public string Cust { get;set;}
        public string Pkg { get; set; }
        public int LC { get; set; }
        public string Device { get; set; }
        public string LotNo { get; set; }
        public string YearCode { get; set; }
        public int QtyIssue { get; set; }
        public int QtyAssyLoss { get; set; }
        public int QtyAssyIn { get; set; }
        public int QtyNonAssyLoss { get; set; }
        public int DieDiscrepency { get; set; }
        public int QtyOut { get; set; }
        public string OverAllYield { get; set; }
        public string AssyYield { get; set; }
        public string CloseDT { get; set; }


    }
}
