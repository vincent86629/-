using angular_API.Model.EFModel;
using angular_API.Model.PageModel;
using angular_API.Model.PageModel.Admin.AdminManage;
using angular_API.Model.PageModel.Enum;
using angular_API.Service.Admin.AdminManage;
using angular_API.Service.Admin.SystemManage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static angular_API.Model.PageModel.FinanceModel;

namespace angular_API.Controllers
{
    [ApiController]
    [Route("api/Finance")]
    [EnableCors("CorsPolicy")]
    public class FinanceController : ControllerBase
    {
        private dbAngular_API_Context _db;
        private IConfiguration _configuration;

        public FinanceController(dbAngular_API_Context db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        /// <summary>
        /// 取得財報列表
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("GetFinanceListData")]
        public FinanceListData GetFinanceListData(FinanceListDataQuery query)
        {
            var resp = new FinanceListData();
            if (query.CommunityId != null)//社區代碼必填
            {
                var rows = _db.TblReport.Include(a => a.Permission)
                                                .Include(a => a.TblReportDetail)
                                                .Include(a => a.TblReportFile)
                                                .Include(a => a.CreateByNavigation)
                                                .Where(a => query.CommunityId == 0 ? true : a.PermissionId == query.CommunityId)
                                                .AsQueryable();
                if (query.StatusId != null)
                    rows = rows.Where(a => a.Status == query.StatusId);
                if (!string.IsNullOrEmpty(query.CreateBy))
                    rows = rows.Where(a => a.CreateByNavigation.Name.Contains(query.CreateBy));
                if (!string.IsNullOrEmpty(query.YearMonth))
                {
                    var yearMonth = query.YearMonth.Replace("/", "");
                    rows = rows.Where(a => a.YearMonth == yearMonth);
                }

                resp.Rows = rows.ToList().Select(a => new FinanceListData.Row
                {
                    Id = a.Id,
                    Community = a.Permission.CodeName,
                    CreateBy = a.CreateByNavigation.Name,
                    Status = GetDescription((FinanceStatus)Enum.ToObject(typeof(FinanceStatus), a.Status)),
                    UpdateTime = a.UpdateTime,
                    YearMonth = a.YearMonth
                }).ToList();
            }
            return resp;
        }
        /// <summary>
        /// 取得財報編輯頁面
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("GetFinanceEditData")]
        public FinanceEditData GetFinanceEditData([FromBody] string id)
        {
            var resp = new FinanceEditData();
            if (int.TryParse(id, out int intId))
                if (intId != 0) //edit
                {
                    var data = _db.TblReport.Include(a => a.Permission)
                                            .Include(a => a.TblReportDetail)
                                            .Include(a => a.TblReportFile)
                                            .Include(a => a.CreateByNavigation)
                                            .FirstOrDefault(a => a.Id == intId);
                    if (data != null)
                    {
                        resp.Id = data.Id;
                        resp.LastMonthBalance = data.LastMonthBalance;
                        resp.Permission = data.Permission.Id.ToString();
                        resp.StatusId = data.Status;
                        resp.ThisMonthBalance = data.ThisMonthBalance;
                        resp.YearMonth = data.YearMonth.Insert(4, "/");
                        resp.CreateBy = data.CreateBy;
                        resp.BankSaving = JsonConvert.DeserializeObject<List<Row>>(data.BankSaving);
                        resp.Blocks = data.TblReportDetail.Select(a => new Block
                        {
                            BlockName = a.BlockName,
                            Total = a.Total ?? 0,
                            TotalName = a.TotalName,
                            Rows = JsonConvert.DeserializeObject<List<Row>>(a.Rows)
                        }).ToList();
                    }
                }
                else //create
                {
                    var emptyRows = new List<Row>();
                    for (int i = 0; i < 15; i++)
                    {
                        emptyRows.Add(new Row());
                    }
                    resp.Blocks.Add(new Block { BlockName = "管理費收入", TotalName = "管理費收入小計" });
                    resp.Blocks.Add(new Block { BlockName = "本月應付款項", TotalName = "本月應付款小計" });
                    resp.Blocks.Add(new Block { BlockName = "銀行匯入", TotalName = "本月管理費/招牌/其他收入" });
                    resp.Blocks.Add(new Block { BlockName = "銀行匯出/扣款", TotalName = "銀行匯出支付小計" });
                    resp.Blocks.Add(new Block { BlockName = "零用金", TotalName = "零用金收入小計" });
                    resp.Blocks.Add(new Block { BlockName = "零用金支出", TotalName = "零用金支出" });
                    foreach (var block in resp.Blocks)
                    {
                        block.Rows.AddRange(emptyRows);
                    }

                    resp.BankSaving.Add(new Row());
                    resp.BankSaving.Add(new Row());
                    resp.BankSaving.Add(new Row());
                    resp.BankSaving.Add(new Row());
                    resp.BankSaving.Add(new Row());
                }

            return resp;
        }
        /// <summary>
        /// 儲存財報
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("SaveFinanceEditData")]
        public APIReturn SaveFinanceEditData(FinanceEditData data)
        {
            var resp = new APIReturn();
            var now = DateTime.Now;
            try
            {
                //新增
                if (data.Id == 0)
                {
                    var report = new TblReport
                    {
                        LastMonthBalance = data.LastMonthBalance,
                        ThisMonthBalance = data.ThisMonthBalance,
                        CreateTime = now,
                        PermissionId = Convert.ToInt32(data.Permission),
                        Status = data.StatusId,
                        UpdateTime = now,
                        YearMonth = data.YearMonth.Replace("/", ""),
                        CreateBy = data.CreateBy,
                        BankSaving = JsonConvert.SerializeObject(data.BankSaving)
                    };
                    _db.TblReport.Add(report);
                    _db.SaveChanges();
                    foreach (var block in data.Blocks)
                    {
                        _db.TblReportDetail.Add(new TblReportDetail
                        {
                            BlockName = block.BlockName,
                            ReportId = report.Id,
                            Rows = JsonConvert.SerializeObject(block.Rows),
                            Total = block.Total,
                            TotalName = block.TotalName
                        });
                    }
                    _db.SaveChanges();
                    resp.Code = APIReturnCode.Success;
                    resp.Message = report.Id.ToString();
                }
                //更新
                else
                {
                    var report = _db.TblReport.Include(a => a.TblReportDetail)
                                              .FirstOrDefault(a => a.Id == data.Id);
                    if (report != null)
                    {
                        report.Id = data.Id;
                        report.LastMonthBalance = data.LastMonthBalance;
                        report.PermissionId = Convert.ToInt32(data.Permission);
                        report.Status = data.StatusId;
                        report.ThisMonthBalance = data.ThisMonthBalance;
                        report.YearMonth = data.YearMonth.Replace("/", "");
                        report.CreateBy = data.CreateBy;
                        report.BankSaving = JsonConvert.SerializeObject(data.BankSaving);
                        report.UpdateTime = now;
                        _db.TblReportDetail.RemoveRange(report.TblReportDetail);
                        foreach (var block in data.Blocks)
                        {
                            _db.TblReportDetail.Add(new TblReportDetail
                            {
                                BlockName = block.BlockName,
                                ReportId = report.Id,
                                Rows = JsonConvert.SerializeObject(block.Rows),
                                Total = block.Total,
                                TotalName = block.TotalName
                            });
                        }
                        _db.SaveChanges();
                        resp.Code = APIReturnCode.Success;
                        resp.Message = report.Id.ToString();
                    }
                    else
                    {
                        resp.Code = APIReturnCode.Fail;
                        resp.Message = "查無此資料";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Code = APIReturnCode.Exception;
                resp.Message = ex.Message;
            }
            return resp;
        }

        /// <summary>
        /// 上傳圖片
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("PictureUpload")]
        public async Task<IActionResult> PictureUpload([FromForm] List<IFormFile> uploads)
        {
            APIReturn result = new APIReturn();
            var TypeName = "FinanceFiles";
            var PicUrl = "";

            try
            {
                if (uploads.Count > 0)
                {
                    var formFile = uploads[0];

                    if (formFile.Length > 0)
                    {
                        var FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        var subFileName = formFile.FileName.Split('.').LastOrDefault();
                        string serverPath = _configuration["ServerPath"];
                        var refPath = $"{FileName}.{subFileName}";

                        bool exists = System.IO.Directory.Exists(serverPath + TypeName);
                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(serverPath + TypeName);
                        }

                        // 要存放的位置
                        var savePath = serverPath + TypeName + "/" + refPath;
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                            PicUrl = refPath;
                        }
                    }

                    return Ok(new APIReturn()
                    {
                        Code = APIReturnCode.Success,
                        Message = _configuration["UploadDomain"] + TypeName + "/" + PicUrl
                    }); ;
                }
                else
                {
                    return Ok(new APIReturn()
                    {
                        Code = APIReturnCode.Fail,
                        Message = "尚未選擇檔案"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new APIReturn()
                {
                    Code = APIReturnCode.Exception,
                    Message = "Error:" + ex.Message + "/Trace:" + ex.StackTrace
                });
            }
        }
        public string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }

}
