using angular_API.Model.EFModel;
using angular_API.Model.PageModel;
using angular_API.Model.PageModel.Admin.AdminManage;
using angular_API.Service.Admin.AdminManage;
using angular_API.Service.Admin.SystemManage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static angular_API.Model.PageModel.FinanceModel;

namespace angular_API.Controllers
{
    [ApiController]
    [Route("api/Finance")]
    [EnableCors("CorsPolicy")]
    public class FinanceController : ControllerBase
    {
        private dbAngular_API_Context _db;

        public FinanceController(dbAngular_API_Context db)
        {
            _db = db;
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
            }
            //更新
            else
            {

            }
            return resp;
        }

    }

}
