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

        public string QueryCloseYieldByLot(QueryDailyYield model)
        {
            CloseYieldByLotALLViewModel vm = new CloseYieldByLotALLViewModel();
            //List <CloseYieldByLotViewModel> vm = new List<CloseYieldByLotViewModel>();
            //List<CloseYieldByLotLossDataViewModel> lossvm = new List<CloseYieldByLotLossDataViewModel>();
            DataBaseConnection data = new DataBaseConnection(this._conn);

            vm.LotView = data.QueryCloseYieldByLotData(model);

            vm.LossData = data.QueryCloseYieldByLotLossData(model);

            foreach (CloseYieldByLotLossDataViewModel LD in vm.LossData)
            {
                foreach(CloseYieldByLotViewModel cyvm in vm.LotView)
                {
                    if(LD.LotNo == cyvm.LotNo)
                    {
                        cyvm.lossDatas.Add(new LossData
                        {
                            LossCode=LD.LossCode,
                            LossDesc=LD.LossDesc,
                            LossQty = LD.LossQty
                        });
                    }
                }
            }
            vm.LossData = vm.LossData.Distinct().ToList();

            return JsonSerializer.Serialize(vm);
        }
        



    }
}
