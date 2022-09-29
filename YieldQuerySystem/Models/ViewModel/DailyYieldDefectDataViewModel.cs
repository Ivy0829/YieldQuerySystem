using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models.ViewModel
{
    public class DailyYieldDefectDataViewModel
    {

        public List<string> ShowTitle { get; set; } = new List<string>();

        public List<DailyYieldDefectDataModel> ShowDefectData { get; set; } = new List<DailyYieldDefectDataModel>();

        public List<DailyYieldDefectDataModel> DefectData { get; set; }

        public List<string> DefectCode { get; set; } = new List<string>();

    }
}
