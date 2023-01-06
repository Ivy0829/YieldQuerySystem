using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YieldQuerySystem.Models.ViewModel
{


    public class CloseYieldSummaryViewModel
    {
        public List<CloseYieldByLotViewModel> YearLotView { get; set; }
        public List<CloseYieldByLotViewModel> MonthLotView { get; set; }
        public List<CloseYieldByLotViewModel> WeeklyLotView { get; set; }
        public List<CloseYieldByLotViewModel> DayLotView { get; set; }

        public List<CloseYieldByLotLossDataViewModel> YearLossDataView { get; set; } = new List<CloseYieldByLotLossDataViewModel>();
        public List<CloseYieldByLotLossDataViewModel> MonthLossDataView { get; set; } = new List<CloseYieldByLotLossDataViewModel>();
        public List<CloseYieldByLotLossDataViewModel> WeeklyLossDataView { get; set; } = new List<CloseYieldByLotLossDataViewModel>();
        public List<CloseYieldByLotLossDataViewModel> DayLossDataView { get; set; } = new List<CloseYieldByLotLossDataViewModel>();

    }
}
