using angular_API.Model.EFModel;
using angular_API.Model.PageModel;
using angular_API.Model.PageModel.Admin.AdminManage;
using angular_API.Service.Admin.AdminManage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace angular_API.Admin.Controllers
{
    [ApiController]
    [Route("api/Admin/AdminPermission")]
    [EnableCors("CorsPolicy")]
    public class AdminPermissionController : ControllerBase
    {
        private AdminPermissionService adminPermissionService;

        public AdminPermissionController(dbAngular_API_Context db)
        {
            adminPermissionService = new AdminPermissionService(db);
        }

        /// <summary>
        /// 取得 權限 列表
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("GetAdminPermissions")]
        public IQueryable<AdminGroupModel> GetAdminPermissions()
        {
            var adminGroups = adminPermissionService.GetAdminPermissions();

            return adminGroups;
        }

        /// <summary>
        /// 取得 權限
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("GetAdminPermissionByID")]
        public AdminGroupModel GetAdminPermissionByID([FromBody]string id)
        {
            var adminGroup = new AdminGroupModel();

            if (int.TryParse(id, out var _id))
            {
                adminGroup = adminPermissionService.GetAdminPermissionByID(_id);
            }

            return adminGroup;
        }

        /// <summary>
        /// 新增/編輯 權限
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("UpdateOrCreateAdminPermission")]
        public APIReturn UpdateOrCreateAdminPermission(AdminGroupModel updateadmingroup)
        {
            APIReturn result = adminPermissionService.UpdateOrCreateAdminPermission(updateadmingroup);

            return result;
        }

        /// <summary>
        /// 刪除 權限
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("DeleteAdminPermission")]
        public APIReturn DeleteAdminPermission([FromBody]string id)
        {
            APIReturn result = new APIReturn();

            if (int.TryParse(id, out var _id))
            {
                result = adminPermissionService.DeleteAdminPermission(_id);
            }
            
            return result;
        }
    }
}
