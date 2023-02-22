using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models.ViewModel
{
    public class VMLossData
    {
        public string LotNo { get; set; }
        public int LossQty { get; set; }
        public int UniLossQty { get; set; }
        public string LossCode { get; set; }
        public string LossDesc { get; set; }
        public string StageCode { get; set; }
        public string YearCode { get; set; }
        public string CloseDT { get; set; }
        public int WeekNumber { get; set; }
    }
    public class CloseYieldByLotLossDataViewModel
    {
        public string StageCode { get; set; }
        public string LossCode { get; set; }
        public string LossDesc { get; set; }
        public int Cum { get; set; }

        public string YearCode { get; set; }

        public string ShowDate { get; set; }

        public List<VMLossData> LD { get; set; } = new List<VMLossData>();




    }
}
