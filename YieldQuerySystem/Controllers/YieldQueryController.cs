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
using System.Reflection;

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
                    dailyYields = dailydata.ToList()
                });
            }

            

            return JsonSerializer.Serialize(vm);




            
        }

        public string QueryDailyYieldDefectData(QueryDailyYield model)
        {
            DataBaseConnection data = new DataBaseConnection(this._conn);
            List<DailyYieldDefectDataModel> DefectData = data.QueryDailyYieldDefectData(model);

            DailyYieldDefectDataViewModel vm = new DailyYieldDefectDataViewModel();

            Type myType = typeof(DailyYieldDefectDataModel);
            PropertyInfo[] mypropertyInfo = myType.GetProperties();

           foreach(var title in mypropertyInfo)
            {
                vm.ShowTitle.Add(title.Name);
            }

            foreach (var title in DefectData.Select (x => x.DefectName).Distinct().ToList())
            {

                vm.ShowTitle.Add(title);
            }

            //foreach (var dailydata in dailyYieldByStageModels)
            //{

            //    vm.Add(new DailyYieldViewModel
            //    {
            //        StageCode = dailydata.Key,
            //        dailyYields = dailydata.ToList()
            //    });
            //}



            return JsonSerializer.Serialize(vm);





        }
    }
}
