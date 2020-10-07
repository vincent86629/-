using angular_API.Extension;
using angular_API.Model.EFModel;
using angular_API.Model.PageModel;
using angular_API.Model.PageModel.Admin.AdminManage;
using ShindaLibrary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace angular_API.Service.Admin.AdminManage
{
    public class AdminLogService
    {
        private readonly dbAngular_API_Context db;

        public AdminLogService(dbAngular_API_Context db)
        {
            this.db = db;
        }

        public IQueryable<AdminLogResponseModel> GetAdminLogs(AdminLogRequestModel request)
        {
            var events = new List<string>();
            if (request.SelectLogin) { events.Add(AdminLogType.Login.GetDescription()); }
            if (request.SelectLogout) { events.Add(AdminLogType.Logout.GetDescription()); }
            if (request.SelectGiftLog) { events.Add(AdminLogType.GiftLog.GetDescription()); }
            if (request.SelectExpertApprove) { events.Add(AdminLogType.ExpertApprove.GetDescription()); }
            if (request.SelectExpertManage) { events.Add(AdminLogType.ExpertManage.GetDescription()); }
            if (request.SelectQAlog) { events.Add(AdminLogType.QALog.GetDescription()); }

            var _endDate = request.EndDate;
            if (_endDate != null)
            {
                _endDate = new DateTime((int)_endDate?.Year, (int)_endDate?.Month, (int)_endDate?.Day, 23, 59, 59); //搜尋範圍是到當天的最後一秒
            }

            var res = db.TblOperationLog
                       .Join(db.TblAdmin, a => a.Operator, b => b.Id, (a, b) => new { TblOperationLog = a, TblAdmin = b })
                       .Where(a => (request.StartDate == null) ? true : a.TblOperationLog.RequestTime >= request.StartDate)
                       .Where(a => (request.EndDate == null) ? true : a.TblOperationLog.RequestTime <= _endDate)
                       .Where(a => (string.IsNullOrEmpty(request.Operator)) ? true : a.TblAdmin.Name.Contains(request.Operator))
                       .Where(a => (string.IsNullOrEmpty(request.Ip)) ? true : a.TblOperationLog.Ip.Contains(request.Ip))
                       .Where(a => (string.IsNullOrEmpty(request.Remark)) ? true : a.TblOperationLog.Remark.Contains(request.Remark))
                       .Where(a => events.Contains(a.TblOperationLog.Type))
                       .Select(a => new AdminLogResponseModel
                       {
                           Id = a.TblOperationLog.Id,
                           Operator = a.TblAdmin.Name,
                           Event = a.TblOperationLog.Name,
                           Ip = a.TblOperationLog.Ip,
                           Remark = a.TblOperationLog.Remark,
                           RequestTime = a.TblOperationLog.RequestTime
                       })
                       .OrderByDescending(a => a.RequestTime)
                       .ThenByDescending(a => a.Event)
                       .ThenByDescending(a => a.Id);
            return res;
        }

        /// <summary>
        /// 寫入 後台使用者操作 Log 紀錄
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public APIReturn AddAdminLog(AdminLog requestModel)
        {
            var rtnMessage = "";
            var res = new APIReturn(APIReturnCode.Fail, rtnMessage);

            try
            {
                //找出該 操作人員
                var admin = db.TblAdmin.Find(requestModel.Operator);

                //如果有找到
                if (admin?.Id != null)
                {
                    var log = new TblOperationLog()
                    {
                        Operator = admin.Id,
                        Type = requestModel.Type,
                        Code = requestModel.Code,
                        Name = requestModel.Name,
                        Remark = requestModel.Remark,
                        Parameter = requestModel.Parameter,
                        Ip = requestModel.Ip,
                        RequestTime = DateTimeTools.Now(),
                    };

                    db.TblOperationLog.Add(log);

                    db.SaveChanges();

                    res = new APIReturn(APIReturnCode.Success, "後台 Log 寫入成功");
                }
                else
                {
                    res = new APIReturn(APIReturnCode.Fail, "無該使用者");
                }

                return res;
            }
            catch (Exception ex)
            {
                res = new APIReturn(APIReturnCode.Exception, ex);
                return res;
            }
        }
    }
}
