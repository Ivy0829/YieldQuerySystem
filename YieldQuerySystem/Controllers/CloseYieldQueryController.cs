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
        



    }
}
