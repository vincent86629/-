using angular_API.Model.PageModel.Admin.AdminManage;
using angular_API.Model.EFModel;
using angular_API.Service.Admin.AdminManage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using angular_API.Model.PageModel;

namespace angular_API.Admin.Controllers
{
    [ApiController]
    [Route("api/Admin/AdminLog")]
    [EnableCors("CorsPolicy")]
    public class AdminLogController : ControllerBase
    {
        private AdminLogService adminLogService;

        public AdminLogController(dbAngular_API_Context db)
        {
            adminLogService = new AdminLogService(db);
        }

        /// <summary>
        /// 取得後台 Log 列表
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("GetAdminLogs")]
        public IQueryable<AdminLogResponseModel> GetAdminLogs(AdminLogRequestModel requestModel)
        {
            var adminLogs = adminLogService.GetAdminLogs(requestModel);

            return adminLogs;
        }


        /// <summary>
        /// 寫入後台 Log
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("AddAdminLog")]
        public APIReturn AddAdminLog(AdminLog requestModel)
        {
            var ip = Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            requestModel.Ip = ip;

            var res = adminLogService.AddAdminLog(requestModel);

            return res;
        }

    }
}
