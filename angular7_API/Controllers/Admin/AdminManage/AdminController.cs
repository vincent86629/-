using angular_API.Model.EFModel;
using angular_API.Model.PageModel;
using angular_API.Model.PageModel.Admin.AdminManage;
using angular_API.Service.Admin.AdminManage;
using angular_API.Service.Admin.SystemManage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace angular_API.Admin.Controllers
{
    [ApiController]
    [Route("api/Admin/Admin")]
    [EnableCors("CorsPolicy")]
    public class AdminController : ControllerBase
    {
        private AdminService adminService;

        public AdminController(dbAngular_API_Context db)
        {
            adminService = new AdminService(db);
        }

        /// <summary>
        /// 取得使用者的選單
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("GetAdminMenuByID")]
        public List<AdminMenuModel> GetAdminMenuByID([FromBody]string id)
        {
            var admins = new List<AdminMenuModel>();

            if (int.TryParse(id, out var _id))
            {
                admins = adminService.GetAdminMenu(_id);
            }

            return admins;
        }

        /// <summary>
        /// 取得使用者列表
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("GetAdmins")]
        public IQueryable<AdminModel> GetAdmins()
        {
            var admins = adminService.GetAdmins();

            return admins;
        }

        /// <summary>
        /// 取得使用者
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("GetAdminByID")]
        public AdminModel GetAdminByID([FromBody]string id)
        {
            AdminModel adminModel = new AdminModel();

            if (int.TryParse(id, out var _id))
            {
                adminModel = adminService.GetAdminByID(_id);
            }

            return adminModel;
        }

        /// <summary>
        /// 新增/編輯 使用者
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("UpdateAdmin")]
        public APIReturn UpdateAdmin(AdminModel updateAdminModel)
        {
            APIReturn result = adminService.UpdateOrCreateAdmin(updateAdminModel);

            return result;
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("DeleteAdmin")]
        public APIReturn DeleteAdmin([FromBody]string id)
        {
            APIReturn result = new APIReturn();

            if (int.TryParse(id, out var _id))
            {
                result = adminService.DeleteAdmin(_id);
            }

            return result;
        }

    }

}
