using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YieldQuerySystem.Models;
using YieldQuerySystem.Models.ViewModel;
using YieldQuerySystem.Models.DAL;
using System.Data;

namespace YieldQuerySystem.Controllers
{
    public class YiledQueryController : Controller
    {

        private readonly IDbConnection _conn;

        public YiledQueryController(IDbConnection conn)
        {
            this._conn = conn;
        }

        public List<DailyYieldViewModel> QueryDailyYieldByStage(QueryDailyYield model)
        {

            List<DailyYieldViewModel> vm = new List<DailyYieldViewModel>();
            DataBaseConnection data = new DataBaseConnection(this._conn);
            vm = data.QueryDailyYieldByStage(model);

            return vm;




            
        }
    }
}
