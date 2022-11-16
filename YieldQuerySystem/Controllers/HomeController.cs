using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using YieldQuerySystem.Models;
using YieldQuerySystem.Models.DAL;
using YieldQuerySystem.Models.ViewModel;

namespace YieldQuerySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDbConnection _conn;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(IDbConnection conn)
        {
            this._conn = conn;
        }

        //public IActionResult YieldSearchView()
        //{
        //    DailyYieldSearchViewModel vm = new DailyYieldSearchViewModel();
        //    DataBaseConnection db = new DataBaseConnection(this._conn);
        //    vm = db.SearchDataforDailyYield();
        //    return View(vm);
        //}

        public IActionResult CloseYieldbyDay()
        {
            DailyYieldSearchViewModel vm = new DailyYieldSearchViewModel();
            DataBaseConnection db = new DataBaseConnection(this._conn);
            vm = db.SearchDataforDailyYield();
            return View(vm);
        }


        public IActionResult CloseYieldbyLot()
        {
            DailyYieldSearchViewModel vm = new DailyYieldSearchViewModel();
            DataBaseConnection db = new DataBaseConnection(this._conn);
            vm = db.SearchDataforDailyYield();
            return View(vm);
        }
        public IActionResult DailyDefect()
        {

            DailyYieldSearchViewModel vm = new DailyYieldSearchViewModel();
            DataBaseConnection db = new DataBaseConnection(this._conn);
            vm = db.SearchDataforDailyYield();
            return View(vm);
        }
        public IActionResult DailyYield()
        {
            //List<DailyYieldViewModel> vm = new List<DailyYieldViewModel>();
            //List<DailyYieldSearchModel> vm = new List<DailyYieldSearchModel>();

            DailyYieldSearchViewModel vm = new DailyYieldSearchViewModel();
            DataBaseConnection db = new DataBaseConnection(this._conn);
            vm = db.SearchDataforDailyYield();


            return View(vm);
        }
        public IActionResult LowYieldSetting()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
