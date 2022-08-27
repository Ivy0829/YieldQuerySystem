using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models.ViewModel
{
    public class DailyYieldByStageModel
    {
        public string StageCode { get; set; }

        public int InQty { get; set; }

        public int OutQty { get; set; }

        public int DefectQty { get; set; }

        public string Yield { get; set; }

        public DateTime OutTime { get; set; }

        public String ShowTime { get; set; }

    }
}
