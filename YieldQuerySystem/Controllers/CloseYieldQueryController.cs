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

namespace YieldQuerySystem.Controllers
{
    public class CloseYieldQueryController : Controller
    {
        private readonly IDbConnection _conn;

        public CloseYieldQueryController(IDbConnection conn)
        {
            this._conn = conn;
        }

        public string QueryYieldByLot(QueryDailyYield model)
        {
            List <CloseYieldByLotViewModel> vm = new List<CloseYieldByLotViewModel>();

            DataBaseConnection data = new DataBaseConnection(this._conn);

            vm = data.QueryCloseYieldByLotData(model);

            return JsonSerializer.Serialize(vm);
        }



    }
}
