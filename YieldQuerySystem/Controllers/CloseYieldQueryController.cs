using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using YieldQuerySystem.Models.ViewModel;
using YieldQuerySystem.Models.DAL;
using YieldQuerySystem.Models;
using System.Text.Json;
using System.Globalization;

namespace YieldQuerySystem.Controllers
{
    public class CloseYieldQueryController : Controller
    {
        private readonly IDbConnection _conn;
        private void GetWeekNumber(ref List<CloseYieldByLotViewModel> LotView)
        {
            CultureInfo myCI = new CultureInfo("zh-TW");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            foreach(var data in LotView)
            {
                data.WeekNumber = myCal.GetWeekOfYear(Convert.ToDateTime(data.CloseDT), myCWR, myFirstDOW);
            }
            
        }

        public CloseYieldQueryController(IDbConnection conn)
        {
            this._conn = conn;
        }

        public string QueryCloseYieldByLot(QueryDailyYield model)
        {
            CloseYieldByLotALLViewModel vm = new CloseYieldByLotALLViewModel();
            //List <CloseYieldByLotViewModel> vm = new List<CloseYieldByLotViewModel>();
            //List<CloseYieldByLotLossDataViewModel> lossvm = new List<CloseYieldByLotLossDataViewModel>();
            DataBaseConnection data = new DataBaseConnection(this._conn);

            vm.LotView = data.QueryCloseYieldByLotData(model);

            vm.LossData = data.QueryCloseYieldByLotLossData(model);

            //foreach (CloseYieldByLotLossDataViewModel LD in vm.LossData)
            //{
            //    foreach(CloseYieldByLotViewModel cyvm in vm.LotView)
            //    {
            //        if(LD.LotNo == cyvm.LotNo)
            //        {
            //            cyvm.lossDatas.Add(new LossData
            //            {
            //                LossCode=LD.LossCode,
            //                LossDesc=LD.LossDesc,
            //                LossQty = LD.LossQty
            //                StageCode=LD.StageCode
            //            });
            //        }
            //    }
            //}
            foreach (VMLossData VMLD in vm.LossData)
            {
                if(!(vm.LossDataView.Exists(x => x.StageCode == VMLD.StageCode) && vm.LossDataView.Exists(x => x.LossCode == VMLD.LossCode)))
                {
                    vm.LossDataView.Add(new CloseYieldByLotLossDataViewModel
                    {
                        StageCode=VMLD.StageCode,
                        LossCode = VMLD.LossCode,
                        LossDesc = VMLD.LossDesc,
                    });
                }
            }
            foreach (var LDV in vm.LossDataView)
            {
                foreach (VMLossData VMLD in vm.LossData)
                {
                    if ((LDV.StageCode == VMLD.StageCode) && (LDV.LossCode == VMLD.LossCode))
                    {
                        LDV.Cum += VMLD.UniLossQty;
                        LDV.LD.Add(VMLD);
                    }
                }
            }
                vm.LossData = vm.LossData.Distinct().ToList();

            return JsonSerializer.Serialize(vm);
        }

        public string QueryCloseYieldSummary(QueryDailyYield model)
        {


            DataBaseConnection data = new DataBaseConnection(this._conn);

            model.StartYearCode = model.StartTime.ToString().Substring(3, 1);
            model.EndYearCode = model.EndTime.ToString().Substring(3, 1);

            List<CloseYieldByLotViewModel> LotView = data.QueryCloseYieldByLotData(model);

            List<VMLossData> LossData = data.QueryCloseYieldByLotLossData(model);

            GetWeekNumber(ref LotView);

            CloseYieldSummaryViewModel vm = new CloseYieldSummaryViewModel();

            vm.YearLotView = LotView.GroupBy(x => new { YearCode = x.YearCode }).Select
                                                                              (x => new CloseYieldByLotViewModel
                                                                              {
                                                                                  LossQty = x.Sum(y => y.LossQty),
                                                                                  QtyIssue = x.Sum(y => y.QtyIssue),
                                                                                  QtyOut = x.Sum(y => y.QtyOut),
                                                                                  ShowDate=x.FirstOrDefault().CloseDT.ToString().Substring(8,2),
                                                                                  YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2)
                                                                              }).ToList();
            vm.MonthLotView = LotView.GroupBy(x => new { CloseDT = x.CloseDT.Substring(0,2) }).Select
                                                                              (x => new CloseYieldByLotViewModel
                                                                              {
                                                                                  LossQty = x.Sum(y => y.LossQty),
                                                                                  QtyIssue = x.Sum(y => y.QtyIssue),
                                                                                  QtyOut = x.Sum(y => y.QtyOut),
                                                                                  ShowDate = Convert.ToDateTime(x.FirstOrDefault().CloseDT).ToString("MMM",new CultureInfo("en-us")).Substring(0, 3),
                                                                                  YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2)
                                                                              }).ToList();
            vm.WeeklyLotView = LotView.GroupBy(x => new { WeekNumber= x.WeekNumber }).Select
                                                                              (x => new CloseYieldByLotViewModel
                                                                              {
                                                                                  LossQty = x.Sum(y => y.LossQty),
                                                                                  QtyIssue = x.Sum(y => y.QtyIssue),
                                                                                  QtyOut = x.Sum(y => y.QtyOut),
                                                                                  ShowDate = x.FirstOrDefault().WeekNumber.ToString(),
                                                                                  YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2)
                                                                              }).ToList();
            vm.DayLotView = LotView.GroupBy(x => new { CloseDT = x.CloseDT.Substring(0, 4) }).Select
                                                                              (x => new CloseYieldByLotViewModel
                                                                              {
                                                                                  LossQty = x.Sum(y => y.LossQty),
                                                                                  QtyIssue = x.Sum(y => y.QtyIssue),
                                                                                  QtyOut = x.Sum(y => y.QtyOut),
                                                                                  ShowDate = Convert.ToDateTime(x.FirstOrDefault().CloseDT).ToString("MMM", new CultureInfo("en-us")).Substring(0, 3),
                                                                                  YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2)
                                                                              }).ToList();

            return JsonSerializer.Serialize("");

        }



    }
}
