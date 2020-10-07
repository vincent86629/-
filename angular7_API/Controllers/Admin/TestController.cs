using angular_API.Model.EFModel;
using angular_API.Model.PageModel;
using angular_API.Service.Admin.AdminManage;
using angular_API.Service.Admin.SystemManage;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace angular_API.Test.Controllers
{
    [ApiController]
    [Route("api/Test")]
    [EnableCors("CorsPolicy")]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        /// <summary>
        /// Test
        /// </summary>
        /// <returns></returns>
        //[DisableCors]
        [HttpGet]
        [Route("Index")]
        public string Index()
        {
            var response = "Test";
            return response;
        }

    }
}
