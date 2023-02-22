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

        private void GetWeekNumber(ref List<VMLossData> LossData)
        {
            CultureInfo myCI = new CultureInfo("zh-TW");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            foreach (var data in LossData)
            {
                data.WeekNumber = myCal.GetWeekOfYear(Convert.ToDateTime(data.CloseDT), myCWR, myFirstDOW);
            }
        }

        private int GetWeekNumber(DateTime time)
        {
            CultureInfo myCI = new CultureInfo("zh-TW");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            return myCal.GetWeekOfYear(Convert.ToDateTime(time), myCWR, myFirstDOW);
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
            GetWeekNumber(ref LossData);

            CloseYieldSummaryViewModel vm = new CloseYieldSummaryViewModel();

            for (int i = Int32.Parse(model.StartYearCode); i <= Int32.Parse(model.EndYearCode);i++)
            {
                vm.Config.YearConfig.Add(2.ToString() +i.ToString());
            }
            string[] MonthAry = { "0", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            for (int i = 3; i>=0; i--)
            {
                if((model.EndTime.Month - i)<0)
                { 
                    vm.Config.MonthConfig.Add(MonthAry[model.EndTime.Month-i+12]+"'"+((model.EndTime.Year % 100) - 1).ToString());
                }
                else 
                {
                    vm.Config.MonthConfig.Add(MonthAry[model.EndTime.Month - i ] + "'" + ((model.EndTime.Year % 100)).ToString());
                }
            }

            int weeklyNum = GetWeekNumber(model.EndTime);
            for (int i = 3; i >= 0; i--)
            {
                if ((weeklyNum - i) < 0)
                {
                    vm.Config.WeeklyConfig.Add((weeklyNum - i + 52).ToString() + "'" + ((model.EndTime.Year % 100) - 1).ToString());
                }
                else
                {
                    vm.Config.WeeklyConfig.Add((weeklyNum - i).ToString() + "'" + ((model.EndTime.Year % 100) ).ToString());
                }
            }


            for (int i = 6; i >= 0; i--)
            {
                if (model.EndTime.Month.ToString().Length < 2)
                {
                    vm.Config.DayConfig.Add("0"+model.EndTime.Month.ToString() + "/" + (model.EndTime.Day-i).ToString());
                }
                else
                {
                    vm.Config.DayConfig.Add(model.EndTime.Month.ToString() + "/" + (model.EndTime.Day-i).ToString());
                }
            }


            vm.YearLotView = LotView.GroupBy(x => new { YearCode = x.YearCode }).Select
                                                                              (x => new CloseYieldByLotViewModel
                                                                              {
                                                                                  LossQty = x.Sum(y => y.LossQty),
                                                                                  QtyIssue = x.Sum(y => y.QtyIssue),
                                                                                  QtyOut = x.Sum(y => y.QtyOut),
                                                                                  ShowDate=x.FirstOrDefault().CloseDT.ToString().Substring(8,2),
                                                                                  YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2)
                                                                              }).ToList();
            vm.MonthLotView = LotView.GroupBy(x => new { YearCode = x.YearCode, CloseDT = x.CloseDT.Substring(0,2) }).Select
                                                                              (x => new CloseYieldByLotViewModel
                                                                              {
                                                                                  LossQty = x.Sum(y => y.LossQty),
                                                                                  QtyIssue = x.Sum(y => y.QtyIssue),
                                                                                  QtyOut = x.Sum(y => y.QtyOut),
                                                                                  ShowDate = Convert.ToDateTime(x.FirstOrDefault().CloseDT).ToString("MMM",new CultureInfo("en-us")).Substring(0, 3),
                                                                                  YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2)
                                                                              }).ToList();
            vm.WeeklyLotView = LotView.GroupBy(x => new { YearCode = x.YearCode, WeekNumber = x.WeekNumber }).Select
                                                                              (x => new CloseYieldByLotViewModel
                                                                              {
                                                                                  LossQty = x.Sum(y => y.LossQty),
                                                                                  QtyIssue = x.Sum(y => y.QtyIssue),
                                                                                  QtyOut = x.Sum(y => y.QtyOut),
                                                                                  ShowDate = x.FirstOrDefault().WeekNumber.ToString(),
                                                                                  YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2)
                                                                              }).ToList();
            vm.DayLotView = LotView.GroupBy(x => new { YearCode = x.YearCode, CloseDT = x.CloseDT.Substring(0, 4) }).Select
                                                                              (x => new CloseYieldByLotViewModel
                                                                              {
                                                                                  LossQty = x.Sum(y => y.LossQty),
                                                                                  QtyIssue = x.Sum(y => y.QtyIssue),
                                                                                  QtyOut = x.Sum(y => y.QtyOut),
                                                                                  ShowDate = (x.FirstOrDefault().CloseDT).Substring(0, 5),
                                                                                  //ShowDate = Convert.ToDateTime(x.FirstOrDefault().CloseDT).ToString("MMM", new CultureInfo("en-us")).Substring(0, 3),
                                                                                  YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2)
                                                                              }).ToList();


            //vm.Yearconfig = (LossData.Select(x => x.YearCode).Distinct().ToList());

            vm.YearLossDataView = LossData.GroupBy(x => new { YearCode = x.YearCode,StageCode=x.StageCode,LossCode=x.LossCode }).Select
                                                                              (x => new CloseYieldByLotLossDataViewModel
                                                                              {
                                                                                  StageCode=x.FirstOrDefault().StageCode,
                                                                                  YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2),
                                                                                  LossCode = x.FirstOrDefault().LossCode,
                                                                                  LossDesc = x.FirstOrDefault().LossDesc,
                                                                                  ShowDate = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2),
                                                                                  Cum = x.Sum(y=>y.UniLossQty)
                                                                              }).ToList();
            vm.MonthLossDataView = LossData.GroupBy(x => new { YearCode = x.YearCode, StageCode = x.StageCode, LossCode = x.LossCode, CloseDT = x.CloseDT.Substring(0, 2) }).Select
                                                                  (x => new CloseYieldByLotLossDataViewModel
                                                                  {
                                                                      StageCode = x.FirstOrDefault().StageCode,
                                                                      YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8,2),
                                                                      LossCode = x.FirstOrDefault().LossCode,
                                                                      LossDesc = x.FirstOrDefault().LossDesc,
                                                                      ShowDate=Convert.ToDateTime(x.FirstOrDefault().CloseDT).ToString("MMMM",new CultureInfo("en-us")).Substring(0,3),
                                                                      Cum = x.Sum(y => y.UniLossQty)
                                                                  }).ToList();
        vm.WeeklyLossDataView = LossData.GroupBy(x => new { YearCode = x.YearCode, StageCode = x.StageCode, LossCode = x.LossCode,WeekNumber = x.WeekNumber }).Select
                                                                  (x => new CloseYieldByLotLossDataViewModel
                                                                  {
                                                                      StageCode = x.FirstOrDefault().StageCode,
                                                                      YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2),
                                                                      LossCode = x.FirstOrDefault().LossCode,
                                                                      LossDesc = x.FirstOrDefault().LossDesc,
                                                                      ShowDate=x.FirstOrDefault().WeekNumber.ToString(),
                                                                      Cum = x.Sum(y => y.UniLossQty)
                                                                  }).ToList();
        vm.DayLossDataView = LossData.GroupBy(x => new { YearCode = x.YearCode, StageCode = x.StageCode, LossCode = x.LossCode, WeekNumber = x.WeekNumber }).Select
                                                          (x => new CloseYieldByLotLossDataViewModel
                                                          {
                                                              StageCode = x.FirstOrDefault().StageCode,
                                                              YearCode = x.FirstOrDefault().CloseDT.ToString().Substring(8, 2),
                                                              LossCode = x.FirstOrDefault().LossCode,
                                                              LossDesc = x.FirstOrDefault().LossDesc,
                                                              ShowDate = (x.FirstOrDefault().CloseDT).Substring(0,5),
                                                              Cum = x.Sum(y => y.UniLossQty)
                                                          }).ToList();



            return JsonSerializer.Serialize(vm);

        }



    }
}
