using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models
{
    public class CloseYieldDetailModel
    {
        public string Seq {get;set;}
        public string LotNo { get; set; }
        public string YearCode { get; set; }
        public string SubLotNo { get; set; }
        public string StageCode { get; set; }
        public string LossCode { get; set; }
        public int LossQty { get; set; }
        public string LossDesc { get; set; }
        public string TranDT { get; set; }
        public string OP { get; set; }
        public string MachID { get; set; }
        public string Cust { get;set;}
        public string Pkg { get; set; }
        public int LC { get; set; }
        public string Device { get; set; }

        


    }
}
