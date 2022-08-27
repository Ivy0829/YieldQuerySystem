using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YieldQuerySystem.Models;
using YieldQuerySystem.Models.ViewModel;
using YieldQuerySystem.Models.DAL;
using System.Data;
using System.Text.Json;

namespace YieldQuerySystem.Controllers
{
    public class YieldQueryController : Controller
    {

        private readonly IDbConnection _conn;

        public YieldQueryController(IDbConnection conn)
        {
            this._conn = conn;
        }

        public string QueryDailyYieldByStage(QueryDailyYield model)
        {

            List<DailyYieldViewModel> vm = new List<DailyYieldViewModel>();
            DataBaseConnection data = new DataBaseConnection(this._conn);
            
            List<DailyYieldByStageModel> dailyYields = data.QueryDailyYieldByStage(model);

            var dailyYieldByStageModels = from dailydata in dailyYields
                       group dailydata by dailydata.StageCode into dailydata2
                       orderby dailydata2.Key
                       select dailydata2;

            foreach(var dailydata in dailyYieldByStageModels)
            {

                vm.Add(new DailyYieldViewModel
                {
                    StageCode = dailydata.Key,
                    dailyYield = dailydata.ToList()
                });
            }

            

            return JsonSerializer.Serialize(vm);




            
        }
    }
}
