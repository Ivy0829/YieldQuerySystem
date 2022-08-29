using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models.ViewModel
{
    public class DailyYieldViewModel
    {
        public string StageCode { get; set; }

        public List<string> OutTime { get; set; }
        public List<DailyYieldByStageModel> dailyYields { get; set; }

    }
}
