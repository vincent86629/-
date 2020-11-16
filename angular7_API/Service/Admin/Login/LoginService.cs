using angular_API.Model.EFModel;
using angular_API.Model.PageModel;
using angular_API.Model.PageModel.Admin.AdminManage;
using angular_API.Service.Admin.AdminManage;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShindaLibrary;
using ShindaLibrary.Tools;
using System;
using System.Linq;
using static angular_API.Model.PageModel.LoginResponse;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace angular_API.Service.Admin.SystemManage
{
    public class LoginService
    {
        private readonly dbAngular_API_Context db;
        private readonly AdminLogService adminLogService;

        IConfiguration _configuration;

        public LoginService(dbAngular_API_Context db, IConfiguration configuration,
            AdminLogService adminLogService)
        {
            this.db = db;
            this.adminLogService = adminLogService;
            _configuration = configuration;
        }

        /// <summary>
        /// 後台管理者登入檢查
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public LoginResponse AdminLoginVefify(LoginRequest loginRequest)
        {
            var response = new LoginResponse();

            try
            {
                //輸入的帳號
                var inputAccount = loginRequest.Account.Trim().ToUpper();
                //尋找該帳號的後台管理者
                var admin = db.TblAdmin
                    .Include(a => a.MapUserPermission)
                    .Where(a => a.Account.ToUpper() == inputAccount && a.IsEnable)
                    .FirstOrDefault();

                //如果有找到該使用者
                if (admin != null)
                {
                    //輸入的密碼
                    var inputPassword = SecurityTools.MD5encrypt(loginRequest.Password.Trim().ToUpper());

                    var isPasswordValid = (admin.Password == inputPassword);
                    if (isPasswordValid) //密碼相符
                    {
                        response.IsLogin = true;
                        response.Messages.Add("登入成功");

                        //寫入 登入成功 Log
                        adminLogService.AddAdminLog(new AdminLog()
                        {
                            Operator = admin.Id,
                            Ip = loginRequest.Ip,
                            Type = "Login",
                            Code = "LoginSuccess",
                            Name = "登入成功",
                            Remark = "",
                        });
                        var permissionIds = admin.MapUserPermission.Select(b => b.PermissionId).ToList();
                        var adminInfo = new AdminInfoModel
                        {
                            Id = admin.Id,
                            Account = admin.Account,
                            Email = admin.Email,
                            Name = admin.Name,
                            IsEnable = admin.IsEnable,
                            Phone = admin.Phone,
                            EmployeeId = admin.EmployeeId,
                            Permissions = db.TblPermission.Where(a => permissionIds.Contains(1) ? true : permissionIds.Contains(a.Id))
                                                               .Select(a => new SelectListItem()
                                                               {
                                                                   Value = a.Id.ToString(),
                                                                   Text = a.CodeName
                                                               }).ToList(),

                        };
                        response.AdminInfo = adminInfo;
                    }
                    else //密碼不符
                    {
                        response.IsLogin = false;
                        response.Messages.Add("登入失敗，請確認帳號密碼輸入正確");
                    }
                }
                else
                {
                    //找不到使用者
                    response.IsLogin = false;
                    response.Messages.Add("無此使用者，請確認帳號密碼輸入正確");
                }
            }
            catch (Exception ex)
            {
                response.IsLogin = false;
                response.Messages.Add("登入失敗");
            }

            return response;
        }


        /// <summary>
        /// 重置使用者密碼
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        public LoginResponse ResetPassword(LoginRequest loginRequest)
        {
            var response = new LoginResponse();

            try
            {
                //輸入的帳號
                var inputAccount = loginRequest.Account.Trim().ToUpper();

                //尋找該帳號的後台管理者
                var admin = db.TblAdmin
                    .Where(a => a.Account.ToUpper() == inputAccount)
                    .FirstOrDefault();

                //如果有找到該使用者
                if (admin != null)
                {
                    //寫入 變更密碼 Log
                    adminLogService.AddAdminLog(new AdminLog()
                    {
                        Operator = admin?.Id ?? 0,
                        Type = "Login",
                        Code = "ResetPassword",
                        Name = "變更密碼",
                        Remark = $"{admin?.Account ?? String.Empty}",
                    });


                    //輸入的信箱
                    var inputEmail = loginRequest.Email.Trim().ToUpper();

                    var isEmailValid = (admin.Email.Trim().ToUpper() == inputEmail);
                    if (isEmailValid) //信箱相符
                    {
                        //如果沒有輸入密碼
                        if (!String.IsNullOrEmpty(loginRequest?.Password?.Trim().ToUpper()))
                        {
                            //輸入的密碼
                            var inputPassword = SecurityTools.MD5encrypt(loginRequest.Password.Trim().ToUpper());

                            //變更成新密碼
                            admin.Password = inputPassword;
                            db.SaveChanges();


                            response.Messages.Add("密碼變更成功");
                        }
                        else
                        {
                            response.Messages.Add("請確認密碼輸入正確");
                        }
                    }
                    else //信箱不符
                    {
                        response.Messages.Add("請確認註冊信箱輸入正確");
                    }
                }
                else
                {
                    //找不到使用者
                    response.Messages.Add("無此使用者，請確認帳號輸入正確");
                }
            }
            catch (Exception ex)
            {
                response.Messages.Add("密碼變更失敗");
            }

            return response;
        }
    }
}
