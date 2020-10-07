using angular_API.Model.EFModel;
using angular_API.Model.PageModel;
using angular_API.Model.PageModel.Admin.AdminManage;
using angular_API.Service.Admin.SystemManage;
using angular_API.Service.Admin.SystemManage;
using Newtonsoft.Json;
using ShindaLibrary.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace angular_API.Service.Admin.AdminManage
{
    public class AdminService
    {
        private readonly dbAngular_API_Context db;
       

        public AdminService(dbAngular_API_Context db)
        {
            this.db = db;
           
        }

        /// <summary>
        /// 取得使用者的選單
        /// </summary>
        /// <returns></returns>
        public List<AdminMenuModel> GetAdminMenu(int AdminId)
        {
            //使用者的權限
            var permissions = db.MapUserPermission
                        .Where(x => x.AdminId == AdminId)
                        .Select(a => a.PermissionId);
            //使用者可進入的頁面
            var menus = db.MapPermissionMenu
                        .Where(x => permissions.Contains(x.PermissionId))
                        .Select(a => a.MenuId)
                        .Distinct();
            //使用這可進入頁面的parent
            var parents = db.MapPermissionMenu
                    .Where(x => permissions.Contains(x.PermissionId))
                    .Select(a => a.Menu.ParentId )
                    .Distinct();

            var res = db.TblMenu
                .Where(x => parents.Contains(x.Id) && x.IsEnable)
                .OrderBy(x => x.Seq)
                .Select(a => new AdminMenuModel()
                {
                    Name = a.Name,
                    Url = a.Url,
                    Childs = db.TblMenu
                            .Where(x => a.Id == x.ParentId && menus.Contains(x.Id) && x.IsEnable)
                            .OrderBy(x => x.Seq)
                            .Select(b => new AdminMenuModel()
                            {
                                Name = b.Name,
                                Url = b.Url
                            }).ToList()
                }).ToList();

            return res;
        }

        /// <summary>
        /// 取得後台使用者清單
        /// </summary>
        /// <returns></returns>
        public IQueryable<AdminModel> GetAdmins()
        {
            var res = db.TblAdmin
                .Select(a => new AdminModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Account = a.Account,
                    Password = a.Password,
                    Email = a.Email,
                    Phone = a.Phone,
                    EmployeeId = a.EmployeeId,
                    IsEnable = a.IsEnable,
                    Groups = db.TblPermission
                     .Where(b => b.IsEnable)
                     .Select(b => new AdminGroupModel
                     {
                         Id = b.Id,
                         CodeName = b.CodeName,
                         IsEnable = b.IsEnable,
                         IsChecked = b.MapUserPermission.Where(c => c.AdminId == a.Id).Any(),
                         //permissionMenus = GetPermissionMenus(new PermissionMenuFilter() { PermissionId = b.Id, ParentId = 0 })
                     }).ToList(),
                    GroupsName = String.Join(',', //把權限名稱用逗號串起來
                        a.MapUserPermission
                        .Join(db.TblPermission, c => c.PermissionId, d => d.Id, (c, d) => new { MapUserPermission = c, TblPermission = d })
                        .Where(e => e.TblPermission.IsEnable)
                        .Select(f => f.TblPermission.CodeName))
                });

            return res;
        }
        //public List<PermissionMenuModel> GetPermissionMenus(PermissionMenuFilter filter)
        //{
        //    dbAngular_API_Context thisdb = new dbAngular_API_Context();

        //    var permissionMenus = thisdb.TblMenu
        //                        .Where(x => x.IsEnable == true && x.ParentId == filter.ParentId)
        //                        .Select(a => new PermissionMenuModel()
        //                        {
        //                            Id = a.Id,
        //                            ParentId = a.ParentId,
        //                            Name = a.Name,
        //                            Child = GetPermissionMenus(new PermissionMenuFilter()
        //                            {
        //                                PermissionId = filter.PermissionId,
        //                                ParentId = a.Id
        //                            }),
        //                            IsChecked = thisdb.MapPermissionMenu.Where(b => b.PermissionId == filter.PermissionId && b.MenuId == a.Id).Any()
        //                        }).ToList();

        //    return permissionMenus;
        //}

        /// <summary>
        /// 根據 ID 取得使用者
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdminModel GetAdminByID(int ID)
        {
            var res = this.GetAdmins()
                .Where(a => a.Id == ID)
                .FirstOrDefault();

            //如果新增使用者的時候也會呼叫，要傳回 Groups 前端才有辦法可以勾選
            if (res == null)
            {
                res = new AdminModel();
                res.Groups = db.TblPermission
                     .Where(b => b.IsEnable)
                     .Select(b => new AdminGroupModel
                     {
                         Id = b.Id,
                         CodeName = b.CodeName,
                         IsEnable = b.IsEnable,
                         IsChecked = false
                     })
                     .ToList();
            }

            return res;
        }

        public APIReturn UpdateOrCreateAdmin(AdminModel UpdateAdminModel)
        {
            string returnMsg = String.Empty;
            var result = new APIReturn(APIReturnCode.Fail, returnMsg);

            try
            {
                //檢查條件
                var isNameEmpty = String.IsNullOrEmpty(UpdateAdminModel.Name);
                var isAccountEmpty = String.IsNullOrEmpty(UpdateAdminModel.Account);
                var isEmailEmpty = String.IsNullOrEmpty(UpdateAdminModel.Email);
                if (isNameEmpty || isAccountEmpty || isEmailEmpty)
                {
                    var errMsgList = new List<string>();
                    if (isNameEmpty)
                    {
                        errMsgList.Add("姓名必填");
                    }

                    if (isAccountEmpty)
                    {
                        errMsgList.Add("帳號必填");
                    }

                    if (isEmailEmpty)
                    {
                        errMsgList.Add("信箱必填");
                    }

                    result = new APIReturn(APIReturnCode.Fail, String.Join('、', errMsgList));
                    return result;
                }

                if (!isEmailEmpty && !Regex.IsMatch(UpdateAdminModel.Email, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$"))
                {
                    result = new APIReturn(APIReturnCode.Fail, "Email 格式不正確");
                    return result;
                }

                //找出該筆 admin
                var admin = db.TblAdmin.Find(UpdateAdminModel.Id);

                //編輯
                if (admin != null)
                {
                    var _tempAccount = UpdateAdminModel.Account.Trim().ToUpper();
                    if (_tempAccount != admin.Account.Trim().ToUpper() && //如果不是原本的那個帳號，改成已存在的帳號
                        db.TblAdmin.Where(a => a.Account.ToUpper() == _tempAccount).FirstOrDefault() != null)
                    {
                        result = new APIReturn(APIReturnCode.Fail, "帳號已存在");
                        return result;
                    }

                    //更新資料
                    admin.Name = UpdateAdminModel.Name;
                    admin.Account = UpdateAdminModel.Account;
                    admin.Email = UpdateAdminModel.Email;
                    admin.Phone = UpdateAdminModel.Phone;
                    admin.EmployeeId = UpdateAdminModel.EmployeeId;
                    admin.IsEnable = UpdateAdminModel.IsEnable;

                    //權限mapping表，先刪除再新增
                    var delmap = db.MapUserPermission
                        .Where(x => x.AdminId == UpdateAdminModel.Id);

                    db.MapUserPermission.RemoveRange(delmap);

                    //新增 權限 Mapping
                    var maps = new List<MapUserPermission>();
                    var groups = UpdateAdminModel.Groups.Where(x => x.IsChecked == true);
                    foreach (var group in groups)
                    {
                        maps.Add(new MapUserPermission()
                        {
                            AdminId = UpdateAdminModel.Id,
                            PermissionId = group.Id
                        });
                    }
                    db.MapUserPermission.AddRange(maps);
                    //寫入 DB
                    db.SaveChanges();

                    returnMsg = "編輯成功";
                }
                else
                {
                    var _tempAccount = UpdateAdminModel.Account.Trim().ToUpper();
                    if (db.TblAdmin.Where(a => a.Account.ToUpper() == _tempAccount).FirstOrDefault() != null)
                    {
                        result = new APIReturn(APIReturnCode.Fail, "帳號已存在");
                        return result;
                    }

                    var initPassword = new Random().Next(0, 99999999).ToString("00000000").ToUpper();

                    //新增
                    var newadmin = new TblAdmin()
                    {
                        Name = UpdateAdminModel.Name,
                        Account = UpdateAdminModel.Account,
                        Email = UpdateAdminModel.Email,
                        Phone = UpdateAdminModel.Phone,
                        EmployeeId = UpdateAdminModel.EmployeeId,
                        IsEnable = UpdateAdminModel.IsEnable,
                        Password = SecurityTools.MD5encrypt(initPassword), //建立隨機預設密碼
                        IsPasswordConfirmed = true, //重置密碼用的，表示密碼沒有重置
                    };

                    //新增 權限 Mapping
                    var maps = new List<MapUserPermission>();
                    var groups = UpdateAdminModel.Groups.Where(x => x.IsChecked);

                    foreach (var group in groups)
                    {
                        newadmin.MapUserPermission.Add(new MapUserPermission()
                        {
                            AdminId = UpdateAdminModel.Id,
                            PermissionId = group.Id
                        });
                    }

                    db.TblAdmin.Add(newadmin);
                    //寫入 DB
                    db.SaveChanges();

                    returnMsg = "新增成功";

                    //發通知
                    //NotificationService.AddNotificationWithSend(
                    //    new TblNotification()
                    //    {
                    //        //Id
                    //        Type = "新增後台帳號通知",
                    //        Subject = "新增後台帳號通知",
                    //        Body = $"您的帳號 {newadmin.Account} 已開通，預設密碼為: {initPassword}，如須變更預設密碼，請至後台點選忘記密碼。",
                    //        Recipient = !String.IsNullOrEmpty(newadmin.Email) ?
                    //            JsonConvert.SerializeObject(new string[] { newadmin.Email }) : JsonConvert.SerializeObject(new string[] { }),
                    //        Parameter = JsonConvert.SerializeObject(new string[] { }),
                    //    }
                    //    , true);
                }

                result = new APIReturn(APIReturnCode.Success, returnMsg);
                return result;
            }
            catch (Exception ex)
            {
                result = new APIReturn(APIReturnCode.Exception, ex);
                return result;
            }
        }


        /// <summary>
        /// 傳入 ID 刪除指定 Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APIReturn DeleteAdmin(int ID)
        {
            try
            {
                //找出該筆 admin
                var admin = db.TblAdmin.Find(ID);

                //刪除
                db.TblAdmin.Remove(admin);

                //寫進 DB
                db.SaveChanges();

                var result = new APIReturn(APIReturnCode.Success, "刪除成功");
                return result;
            }
            catch (Exception ex)
            {
                var result = new APIReturn(APIReturnCode.Exception, ex);
                return result;
            }
        }
    }
}
