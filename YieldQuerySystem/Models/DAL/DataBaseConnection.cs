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
        
        public List<DailyYieldViewModel> QueryDailyYieldByStage(QueryDailyYield model)
        {

            List <DailyYieldViewModel> vm = new List<DailyYieldViewModel>();
            this._conn.Open();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Plant", "abc", DbType.String, ParameterDirection.Input);
            parameters.Add("@Cust2Code", "abc", DbType.String, ParameterDirection.Input);
            parameters.Add("@Cust3Code", "abc", DbType.String, ParameterDirection.Input);
            parameters.Add("@PKGCode", "abc", DbType.String, ParameterDirection.Input);
            parameters.Add("@DeviceName", "abc", DbType.String, ParameterDirection.Input);
            parameters.Add("@StartTime", "abc", DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@EndTime", "abc", DbType.DateTime, ParameterDirection.Input);

            var result = this._conn.Query<DailyYieldViewModel>("[dbo].[SP_DailyYieldByStage", parameters, commandType: CommandType.StoredProcedure);
            vm = result.ToList();
            return vm;


        }



    }
}
