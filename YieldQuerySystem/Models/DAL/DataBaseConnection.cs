using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using Dapper.FluentColumnMapping;
using YieldQuerySystem.Models.ViewModel;

namespace YieldQuerySystem.Models.DAL
{
    public class DataBaseConnection
    {
        private readonly IDbConnection _conn;

        public DataBaseConnection(IDbConnection conn)
        {
            this._conn = conn;
        }

        public bool InsertList(List<DailyYieldModel> modellist, List<DailyYieldDetailModel> DetailModellist)
        {
            bool result = false;
            this._conn.Open();
            //var columnMappings = new ColumnMappingCollection();

            //columnMappings.RegisterType<DailyYieldModel>()
            //              .MapProperty()
            using (var tran = this._conn.BeginTransaction())
            {
                try
                {
                    var sql = @"INSERT INTO [dbo].[DailyYield]
                ([Guid],[YearCode],[Plant],[SubLotNo],[LotNo],[StageCode],[Cust2Code],[Cust3Code],[PkgCode],[Device]
                ,[TrackInTime],[TrackInQty],[TrackOutTime],[TrackOutQty],[SumDefectQty],[RunType],[Yield])
                VALUES
                    (@Guid, @YearCode, @Plant, @SubLotNo, @LotNo, @StageCode, @Cust2Code, @Cust3Code, @PkgCode, @Device
                    ,@TrackInTime, @TrackInQty, @TrackOutTime, @TrackOutQty, @SumDefectQty, @RunType, @Yield)";

                    var Detailsql = @"INSERT INTO [dbo].[DailyYieldDetail]
                   ([Guid],[DefectName],[DefectQty])
                   VALUES(@Guid,@DefectName,@DefectQty)";

                    this._conn.Execute(sql, modellist, tran);
                    this._conn.Execute(Detailsql, DetailModellist, tran);
                    tran.Commit();
                    result = true;

                }
                //    foreach (DailyYieldModel model in modellist)
                //    {
                //        this._conn.Execute(sql, model);
                //    }
                //    foreach (DailyYieldDetailModel model in DetailModellist)
                //    {
                //        this._conn.Execute(Detailsql, DetailModellist);
                //    }
                //    tran.Commit();
                //}
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;
                }
            }
            this._conn.Close();
            return result;
        }

        public bool InsertCloseYieldByLotData(List<CloseYieldModel> DataList)
        {
            bool result = false;
            this._conn.Open();
            using (var tran = this._conn.BeginTransaction())
            {
                try
                {
                    var sql = @"INSERT INTO [dbo].[CloseYield]
                ([Fac],[Cust],[Pkg],[LC],[Device],[LotNo],[YearCode],[QtyIssue],[QtyAssyLoss],[QtyAssyIn]
                ,[QtyNonAssyLoss],[DieDiscrepency],[QtyOut],[OverAllYield],[AssyYield],[CloseDT])
                VALUES
                (@Fac,@Cust,@Pkg,@LC,@Device,@LotNo,@YearCode,@QtyIssue,@QtyAssyLoss,@QtyAssyIn
                ,@QtyNonAssyLoss,@DieDiscrepency,@QtyOut,@OverAllYield,@AssyYield,@CloseDT)";

                    this._conn.Execute(sql, DataList, tran);
                    tran.Commit();
                    result = true;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw;
                }
            }
            return result;
        }


        public bool InsertCloseYieldDefectData(List<CloseYieldDetailModel> DataList)
        {
            bool result = false;
            this._conn.Open();
            using (var tran = this._conn.BeginTransaction())
            {
                try
                {
                    var sql = @"INSERT INTO [dbo].[CloseYieldDetail]
                ([LotNo],[YearCode],[SubLotNo],[StageCode],[LossCode],[LossQty],[TranDT],[OP],[MachID],[Cust],[Pkg],[LC],[Device])
                VALUES
                (@LotNo,@YearCode,@SubLotNo,@StageCode,@LossCode,@LossQty,@TranDT,@OP,@MachID,@Cust,@Pkg,@LC,@Device)";

                    this._conn.Execute(sql, DataList, tran);
                    tran.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;
                }
            }
            return result;
        }




        public List<DailyYieldByStageModel> QueryDailyYieldByStage(QueryDailyYield model)
        {

            List <DailyYieldByStageModel> vm = new List<DailyYieldByStageModel>();
            this._conn.Open();
            DynamicParameters parameters = new DynamicParameters();


            parameters.Add("@Plant", model.Plant, DbType.String, ParameterDirection.Input);
            parameters.Add("@Cust2Code", model.Cust2Code, DbType.String, ParameterDirection.Input);
            parameters.Add("@Cust3Code", model.Cust3Code, DbType.String, ParameterDirection.Input);
            parameters.Add("@PKGCode", model.PKGCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@StageCode", model.StageCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@DeviceName", model.DeviceName, DbType.String, ParameterDirection.Input);
            parameters.Add("@StartTime", model.StartTime, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@EndTime", model.EndTime, DbType.DateTime, ParameterDirection.Input);

            var result = this._conn.Query<DailyYieldByStageModel>("[dbo].[SP_DailyYieldByStage]", parameters, commandType: CommandType.StoredProcedure);
            vm = result.ToList();

            foreach(var item in vm)
            {
                item.ShowTime = item.OutTime.ToString("MM/dd");
            }

            return vm;


        }

        public List<DailyYieldDefectDataModel> QueryDailyYieldDefectData(QueryDailyYield model)
        {

            List<DailyYieldDefectDataModel> vm = new List<DailyYieldDefectDataModel>();
            this._conn.Open();
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Plant", model.Plant, DbType.String, ParameterDirection.Input);
            parameters.Add("@Cust2Code", model.Cust2Code, DbType.String, ParameterDirection.Input);
            parameters.Add("@Cust3Code", model.Cust3Code, DbType.String, ParameterDirection.Input);
            parameters.Add("@PKGCode", model.PKGCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@StageCode", model.StageCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@DeviceName", model.DeviceName, DbType.String, ParameterDirection.Input);
            parameters.Add("@StartTime", model.StartTime, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@EndTime", model.EndTime, DbType.DateTime, ParameterDirection.Input);

            var result = this._conn.Query<DailyYieldDefectDataModel>("[dbo].[SP_DailyYieldDefectData]", parameters, commandType: CommandType.StoredProcedure);
            vm = result.ToList();

            return vm;


        }

        public DailyYieldSearchViewModel SearchDataforDailyYield()
        {
            DailyYieldSearchViewModel vm = new DailyYieldSearchViewModel();
            List<DailyYieldSearchModel> data = new List<DailyYieldSearchModel>();
            this._conn.Open();
            var result = this._conn.Query<DailyYieldSearchModel>("[dbo].[SP_SearchDataforDailyYield]", commandType: CommandType.StoredProcedure);
           data = result.ToList();

            vm.PlantList = data.Select(x => x.Plant).Distinct().ToList();
            vm.StageCodeList = data.Select(x => x.StageCode).Distinct().ToList();
            vm.Cust2CodeList = data.Select(x => x.Cust2Code).Distinct().ToList();
            vm.Cust3CodeList = data.Select(x => x.Cust3Code).Distinct().ToList();
            vm.PkgCodeList = data.Select(x => x.PkgCode).Distinct().ToList();


            return vm;


        }


        public List<CloseYieldByLotViewModel> QueryCloseYieldByLotData(QueryDailyYield model)
        {

            List<CloseYieldByLotViewModel> vm = new List<CloseYieldByLotViewModel>();
            this._conn.Open();
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Plant", model.Plant, DbType.String, ParameterDirection.Input);
            parameters.Add("@Cust2Code", model.Cust2Code, DbType.String, ParameterDirection.Input);
            parameters.Add("@PKGCode", model.PKGCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@StageCode", model.StageCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@DeviceName", model.DeviceName, DbType.String, ParameterDirection.Input);
            parameters.Add("@StartTime", model.StartTime, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@EndTime", model.EndTime, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@StartYearCode", model.StartYearCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@EndYearCode", model.EndYearCode, DbType.String, ParameterDirection.Input);

            var result = this._conn.Query<CloseYieldByLotViewModel>("[dbo].[SP_CloseYieldByLotData]", parameters, commandType: CommandType.StoredProcedure);
            vm = result.ToList();
            this._conn.Close();
            return vm;
        }

        public List<VMLossData> QueryCloseYieldByLotLossData(QueryDailyYield model)
        {

            List<VMLossData> vm = new List<VMLossData>();
            //List<CloseYieldByLotLossDataViewModel> lossvm = new List<CloseYieldByLotLossDataViewModel>();
            this._conn.Open();
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Plant", model.Plant, DbType.String, ParameterDirection.Input);
            parameters.Add("@Cust2Code", model.Cust2Code, DbType.String, ParameterDirection.Input);
            parameters.Add("@PKGCode", model.PKGCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@StageCode", model.StageCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@DeviceName", model.DeviceName, DbType.String, ParameterDirection.Input);
            parameters.Add("@StartTime", model.StartTime, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@EndTime", model.EndTime, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@StartYearCode", model.StartYearCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@EndYearCode", model.EndYearCode, DbType.String, ParameterDirection.Input);

            var result = this._conn.Query<VMLossData>("[dbo].[SP_CloseYieldByLotLossData]", parameters, commandType: CommandType.StoredProcedure);
            vm = result.ToList();
            this._conn.Close();
            return vm;


        }

    }
}
