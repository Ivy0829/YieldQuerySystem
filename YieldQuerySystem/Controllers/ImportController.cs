using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using YieldQuerySystem.Models;
using System.Collections.Generic;
using YieldQuerySystem.Models.DAL;
using System.Data;

namespace YieldQuerySystem.Controllers
{
    public class ImportController : Controller
    {

        private readonly IDbConnection _conn;

        public ImportController(IDbConnection conn)
        {
            this._conn = conn;
        }
        /// <summary>
        /// 判斷副檔名取得WorkBook實例，取得檔案內容
        /// </summary>
        /// <returns></returns>

        private IWorkbook GetWoekbook(string extension, IFormFile file)
        {
            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);

            Stream stream = new MemoryStream(ms.ToArray());

            switch (extension)
            {
                case ".xls":
                    return new HSSFWorkbook(stream);
                case ".xlsx":
                case ".xlsm":
                    return new XSSFWorkbook(stream);
                default:
                    return null;
            }
        }

        private bool CheckExcelFile(IFormFile file, out IWorkbook wb)
        {
            wb = null;
            //檢查是否有選擇檔案
            if (file != null)
            {
                //檢查是否有選擇檔案
                if (file.Length > 0)
                {
                    //取得副檔名
                    string extension = Path.GetExtension(file.FileName);

                    wb = GetWoekbook(extension, file);

                    if (wb == null)
                    {
                        ViewData["Massage"] = "請上傳.xls or .xlsx or xlsm 的檔案";
                        return false;
                    }
                }
                else
                {
                    ViewData["Massage"] = "上傳失敗，檔案內沒有任何資料";
                    return false;
                }
            }
            else
            {
                ViewData["Massage"] = "請選擇檔案";
                return false;
            }
            return true;
        }
        private bool InsertDailyRawData(List<DailyYieldModel> DailyYieldList, List<DailyYieldDetailModel> DailyYieldDetailList)
        {

            DataBaseConnection data = new DataBaseConnection(this._conn);

            return data.InsertList(DailyYieldList, DailyYieldDetailList);

        }
        private bool InsertCloseYieldByLotData(List<CloseYieldModel> DataList)
        {

            DataBaseConnection data = new DataBaseConnection(this._conn);

            return data.InsertCloseYieldByLotData(DataList);

        }

        [HttpPost]
        public IActionResult UploadDailyRawData(IFormFile file)
        {
            try
            {
                IWorkbook wb;
                if (CheckExcelFile(file, out wb))
                {
                    //取得第一個工作表
                    ISheet sheet = wb.GetSheetAt(0);
                    //取得標題列
                    IRow header = sheet.GetRow(0);

                    List<DailyYieldModel> DailyYieldList = new List<DailyYieldModel>();
                    List<DailyYieldDetailModel> DailyYieldDetailList = new List<DailyYieldDetailModel>();

                    //走訪所有資料列(排除標題列)
                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        string tempguid = Guid.NewGuid().ToString();

                        int tempqtydata = 0;

                        //驗證不是空白列
                        if (sheet.GetRow(row) != null)
                        {

                            //將每一列放入List內
                            DailyYieldList.Add(new DailyYieldModel
                            {
                                Guid = tempguid,
                                YearCode = sheet.GetRow(row).GetCell(0).ToString(),
                                Plant = sheet.GetRow(row).GetCell(1).ToString(),
                                SubLotNo = sheet.GetRow(row).GetCell(2).ToString(),
                                LotNo = sheet.GetRow(row).GetCell(3).ToString(),
                                StageCode = sheet.GetRow(row).GetCell(4).ToString(),
                                Cust2Code = sheet.GetRow(row).GetCell(5).ToString(),
                                Cust3Code = sheet.GetRow(row).GetCell(6).ToString(),
                                PkgCode = sheet.GetRow(row).GetCell(7).ToString(),
                                Device = sheet.GetRow(row).GetCell(8).ToString(),
                                TrackInTime = Convert.ToDateTime(sheet.GetRow(row).GetCell(9).DateCellValue),
                                TrackInQty = Convert.ToInt32(sheet.GetRow(row).GetCell(10).ToString()),
                                TrackOutTime = Convert.ToDateTime(sheet.GetRow(row).GetCell(11).DateCellValue),
                                TrackOutQty = Convert.ToInt32(sheet.GetRow(row).GetCell(12).ToString()),
                                SumDefectQty = Convert.ToInt32(sheet.GetRow(row).GetCell(13).ToString()),
                                RunType = sheet.GetRow(row).GetCell(14).ToString(),
                                Yield = sheet.GetRow(row).GetCell(15).ToString().Length > 6
                                ? sheet.GetRow(row).GetCell(15).ToString().Substring(0, 6)
                                : sheet.GetRow(row).GetCell(15).ToString()
                            });

                            for (int i = 16; i < sheet.GetRow(row).LastCellNum; i++)
                            {
                                tempqtydata = int.Parse(sheet.GetRow(row).GetCell(i).ToString());
                                if (tempqtydata != 0)
                                {
                                    DailyYieldDetailList.Add(new DailyYieldDetailModel
                                    {
                                        Guid = tempguid,
                                        DefectName = header.GetCell(i).ToString(),
                                        DefectQty = tempqtydata
                                    });
                                }
                            }
                        }
                    }

                    if (InsertDailyRawData(DailyYieldList, DailyYieldDetailList))
                    {
                        ViewData["Massage"] = "上傳成功";
                    }
                    else
                    {
                        ViewData["Massage"] = "上傳失敗";
                    }

                }
            }
            catch (Exception ex)
            {
                ViewData["Massage"] = $"上傳失敗，詳細原因:{ex.Message}";
            }
            //return Redirect("~/");
            return View("Index");
        }

        [HttpPost]
        public IActionResult UploadCloseYieldByLotData(IFormFile file)
        {
            try
            {
                IWorkbook wb;
                if (CheckExcelFile(file, out wb))
                {
                    //取得第一個工作表
                    ISheet sheet = wb.GetSheetAt(0);
                    //取得標題列
                    IRow header = sheet.GetRow(0);

                    List<CloseYieldModel> CloseYieldList = new List<CloseYieldModel>();

                    //走訪所有資料列(排除標題列)
                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        //驗證不是空白列
                        if (sheet.GetRow(row) != null)
                        {
                            //將每一列放入List內
                            CloseYieldList.Add(new CloseYieldModel
                            {
                                Fac = sheet.GetRow(row).GetCell(1).ToString(),
                                Cust = sheet.GetRow(row).GetCell(2).ToString(),
                                Pkg = sheet.GetRow(row).GetCell(3).ToString(),
                                LC = Convert.ToInt32(sheet.GetRow(row).GetCell(4).ToString()),
                                Device = sheet.GetRow(row).GetCell(5).ToString(),
                                LotNo = sheet.GetRow(row).GetCell(6).ToString(),
                                YearCode = sheet.GetRow(row).GetCell(7).ToString(),
                                QtyIssue = Convert.ToInt32(sheet.GetRow(row).GetCell(8).ToString()),
                                QtyAssyLoss = Convert.ToInt32(sheet.GetRow(row).GetCell(9).ToString()),
                                QtyAssyIn = Convert.ToInt32(sheet.GetRow(row).GetCell(10).ToString()),
                                QtyNonAssyLoss = Convert.ToInt32(sheet.GetRow(row).GetCell(11).ToString()),
                                DieDiscrepency = Convert.ToInt32(sheet.GetRow(row).GetCell(12).ToString()),
                                QtyOut = Convert.ToInt32(sheet.GetRow(row).GetCell(13).ToString()),
                                OverAllYield = sheet.GetRow(row).GetCell(14).ToString().Length > 6
                                ? sheet.GetRow(row).GetCell(14).ToString().Substring(0, 6)
                                : sheet.GetRow(row).GetCell(14).ToString(),
                                AssyYield = sheet.GetRow(row).GetCell(15).ToString().Length > 6
                                ? sheet.GetRow(row).GetCell(15).ToString().Substring(0, 6)
                                : sheet.GetRow(row).GetCell(15).ToString(),
                                CloseDT = sheet.GetRow(row).GetCell(16).ToString(),


                                
                            });

                         
                        }
                    }
                    if (InsertCloseYieldByLotData(CloseYieldList))
                    {
                        ViewData["Massage"] = "上傳成功";
                    }
                    else
                    {
                        ViewData["Massage"] = "上傳失敗";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Massage"] = $"上傳失敗，詳細原因:{ex.Message}";
            }
            //return Redirect("~/");
            return View("Index");
        }

        //public IActionResult UploadCloseYieldDefectData(IFormFile file)
        //{
        //    try
        //    {
        //        IWorkbook wb;
        //        if (CheckExcelFile(file, out wb))
        //        {
        //            //取得第一個工作表
        //            ISheet sheet = wb.GetSheetAt(0);
        //            //取得標題列
        //            IRow header = sheet.GetRow(0);

        //            List<CloseYieldModel> CloseYieldList = new List<CloseYieldModel>();

        //            //走訪所有資料列(排除標題列)
        //            for (int row = 1; row <= sheet.LastRowNum; row++)
        //            {
        //                //驗證不是空白列
        //                if (sheet.GetRow(row) != null)
        //                {
        //                    //將每一列放入List內
        //                    CloseYieldList.Add(new CloseYieldModel
        //                    {
        //                        Fac = sheet.GetRow(row).GetCell(1).ToString(),
        //                        Cust = sheet.GetRow(row).GetCell(2).ToString(),
        //                        Pkg = sheet.GetRow(row).GetCell(3).ToString(),
        //                        LC = Convert.ToInt32(sheet.GetRow(row).GetCell(4).ToString()),
        //                        Device = sheet.GetRow(row).GetCell(5).ToString(),
        //                        LotNo = sheet.GetRow(row).GetCell(6).ToString(),
        //                        YearCode = sheet.GetRow(row).GetCell(7).ToString(),
        //                        QtyIssue = Convert.ToInt32(sheet.GetRow(row).GetCell(8).ToString()),
        //                        QtyAssyLoss = Convert.ToInt32(sheet.GetRow(row).GetCell(9).ToString()),
        //                        QtyAssyIn = Convert.ToInt32(sheet.GetRow(row).GetCell(10).ToString()),
        //                        QtyNonAssyLoss = Convert.ToInt32(sheet.GetRow(row).GetCell(11).ToString()),
        //                        DieDiscrepency = Convert.ToInt32(sheet.GetRow(row).GetCell(12).ToString()),
        //                        QtyOut = Convert.ToInt32(sheet.GetRow(row).GetCell(13).ToString()),
        //                        OverAllYield = sheet.GetRow(row).GetCell(14).ToString().Length > 6
        //                        ? sheet.GetRow(row).GetCell(14).ToString().Substring(0, 6)
        //                        : sheet.GetRow(row).GetCell(14).ToString(),
        //                        AssyYield = sheet.GetRow(row).GetCell(15).ToString().Length > 6
        //                        ? sheet.GetRow(row).GetCell(15).ToString().Substring(0, 6)
        //                        : sheet.GetRow(row).GetCell(15).ToString(),
        //                        CloseDT = sheet.GetRow(row).GetCell(16).ToString(),



        //                    });


        //                }
        //            }
        //            if (InsertCloseYieldByLotData(CloseYieldList))
        //            {
        //                ViewData["Massage"] = "上傳成功";
        //            }
        //            else
        //            {
        //                ViewData["Massage"] = "上傳失敗";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["Massage"] = $"上傳失敗，詳細原因:{ex.Message}";
        //    }
        //    //return Redirect("~/");
        //    return View("Index");
        //}
        public IActionResult Index()
        {
            return View();
        }




    }
}
