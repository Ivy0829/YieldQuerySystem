using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models.ViewModel
{
    public class DailyYieldDefectDataModel
    {
        public string SubLotNo { get; set; }
        public string LotNo { get; set; }
        public string Cust2Code { get; set; }
        public string Device { get; set; }
        public string StageCode { get; set; }
        public DateTime TrackInTime { get; set; }
        public DateTime TrackOutTime { get; set; }
        public int TrackInQty { get; set; }
        public int TrackOutQty { get; set; }
        public string Yield { get; set; }
        public int SumDefectQty { get; set; }
        public string DefectName { get; set; }
        public string DefectQty { get; set; }

   
    }
}
