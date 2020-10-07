using angular_API.Model.EFModel;
using System;
using System.Collections.Generic;

namespace angular_API.Model.PageModel
{

    //登入頁面用 PageModel
    public class AdminLoginPageModel
    {
        //登入資訊 (帳密)
        public LoginRequest LoginRequest { get;set;}

        //登入人員資訊
        public LoginResponse LoginResponse { get; set; }

        public AdminLoginPageModel()
        {
            this.LoginRequest = new LoginRequest();
            this.LoginResponse = new LoginResponse();
        }
    }

    //登入 Request
    public class LoginRequest
    {
        //帳號
        public string Account { get; set; }
        //密碼
        public string Password { get; set; }
        //信箱
        public string Email { get; set; }
        //記住我
        public bool RememberMe { get; set; }
        //登入IP
        public string Ip { get; set; }

        public LoginRequest()
        {
            this.Account = String.Empty;
            this.Password = String.Empty;
            this.Email = String.Empty;
            this.RememberMe = false;
        }
    }

    //登入 Response
    public class LoginResponse
    {
        //登入成功
        public bool IsLogin { get; set; }

        //登入訊息
        public List<string> Messages { get; set; }

        //人員資訊
        public  TblAdmin AdminInfo { get; set; }

        public LoginResponse()
        {
            this.IsLogin = false;
            this.Messages = new List<string>();
            this.AdminInfo = new TblAdmin();
        }
    }
}
