using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models.ViewModel
{
    public class CloseYieldByLotViewModel
    {
        public string LotNo { get; set; }
        public int QtyIssue { get; set; }
        public int QtyOut { get; set; }
        public string OverAllYield { get; set; }
        public int LossQty { get; set; }
        public string CloseDT { get; set; }

        public List<LossData> lossDatas { get; set; }






    }
}
