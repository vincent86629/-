using angular_API.Model.EFModel;
using angular_API.Model.PageModel;
using angular_API.Service.Admin.AdminManage;
using angular_API.Service.Admin.SystemManage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace angular_API.Admin.Controllers
{
    [ApiController]
    [Route("api/Admin/Login")]
    [EnableCors("CorsPolicy")]
    public class AdminLoginController : ControllerBase
    {
        private LoginService loginService;
        IConfiguration _configuration;

        public AdminLoginController(dbAngular_API_Context db, IConfiguration configuration,
            AdminLogService adminLogService)
        {
            this.loginService = new LoginService(db, configuration, adminLogService);
            this._configuration = configuration;
        }

        /// <summary>
        /// 後台管理者登入驗證
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("AdminLoginVefify")]
        public LoginResponse AdminLoginVefify(LoginRequest loginRequest)
        {
            var ip = Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            loginRequest.Ip = ip;

            var response = loginService.AdminLoginVefify(loginRequest);
            return response;
        }

        /// <summary>
        /// 後台管理者登入驗證
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpPost]
        [Route("ResetPassword")]
        public LoginResponse ResetPassword(LoginRequest loginRequest)
        {
            var response = loginService.ResetPassword(loginRequest);
            return response;
        }
    }
}
