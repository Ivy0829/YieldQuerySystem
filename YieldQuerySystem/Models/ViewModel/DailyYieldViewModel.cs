using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models.ViewModel
{
    public class DailyYieldViewModel
    {
        string StageCode { get; set; }

        int InQty { get; set; }

        int OutQty { get; set; }

        int DefectQty { get; set; }

        string Yield { get; set; }

        string OutTime { get; set; }

    }
}
