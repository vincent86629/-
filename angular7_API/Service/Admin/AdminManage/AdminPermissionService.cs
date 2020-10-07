using angular_API.Model.PageModel;
using angular_API.Model.EFModel;
using System;
using System.Linq;
using angular_API.Model.PageModel.Admin.AdminManage;
using System.Collections.Generic;

namespace angular_API.Service.Admin.AdminManage
{
    public class AdminPermissionService
    {
        private readonly dbAngular_API_Context db;

        public AdminPermissionService(dbAngular_API_Context db)
        {
            this.db = db;
        }

        //取得權限tree
        public List<PermissionMenuModel> GetPermissionMenus(PermissionMenuFilter filter)
        {
            //dbAngular_API_Context thisdb = new dbAngular_API_Context();

            var permissionMenus = db.TblMenu
                                .Where(x => x.IsEnable == true && x.ParentId == filter.ParentId)
                                .Select(a => new PermissionMenuModel()
                                {
                                    Id = a.Id,
                                    ParentId = a.ParentId,
                                    Name = a.Name,
                                    //Child = GetPermissionMenus(new PermissionMenuFilter()
                                    //{
                                    //    PermissionId = filter.PermissionId,
                                    //    ParentId = a.Id
                                    //}),
                                    IsChecked = db.MapPermissionMenu.Where(b => b.PermissionId == filter.PermissionId && b.MenuId == a.Id).Any()
                                }).ToList();

            permissionMenus.ForEach(x => x.Child = GetPermissionMenus(new PermissionMenuFilter()
            {
                PermissionId = filter.PermissionId,
                ParentId = x.Id
            }));

            return permissionMenus;
        }

        /// <summary>
        /// 取得權限列表 (所有的權限項目)
        /// </summary>
        /// <returns></returns>
        public IQueryable<AdminGroupModel> GetAdminPermissions()
        {
            var adminGroups = db.TblPermission
                .Select(a => new AdminGroupModel()
                    {
                        Id = a.Id,
                        CodeName = a.CodeName,
                        IsEnable = a.IsEnable
                    }
                );
            return adminGroups;
        }

        /// <summary>
        /// 傳入 ID 取得指定權限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AdminGroupModel GetAdminPermissionByID(int ID)
        {
            var adminGroup = this.GetAdminPermissions()
                .Where(a => a.Id == ID)
                .FirstOrDefault();
            if (ID == 0) { adminGroup = new AdminGroupModel(); }
            adminGroup.permissionMenus = GetPermissionMenus(new PermissionMenuFilter() { PermissionId = ID, ParentId = 0 });
            return adminGroup;
        }


        /// <summary>
        /// 更新或新增權限
        /// </summary>
        /// <param name="updatepermission"></param>
        /// <returns></returns>
        public APIReturn UpdateOrCreateAdminPermission(AdminGroupModel UpdateAdminGroup)
        {
            string returnMsg = "";
            var result = new APIReturn(APIReturnCode.Fail, "");

            try
            {
               
                //檢查條件
                var isCodeNameEmpty = String.IsNullOrEmpty(UpdateAdminGroup.CodeName);

                if (isCodeNameEmpty)
                {
                    result = new APIReturn(APIReturnCode.Fail, "權限名稱必填");
                    return result;
                }

                //找出該筆 ID
                var permission = db.TblPermission.Find(UpdateAdminGroup.Id);

                //勾選的權限
                var mapPM = UpdateAdminGroup.CheckedMenus.Select(a => new MapPermissionMenu() { MenuId = a }).ToList();

                //編輯
                if (permission != null)
                {
                    //更新
                    permission.CodeName = UpdateAdminGroup.CodeName;
                    permission.IsEnable = UpdateAdminGroup.IsEnable;

                    //map表 先刪除再新增
                    var delmapPM = db.MapPermissionMenu.Where(x => x.PermissionId == permission.Id);
                    db.MapPermissionMenu.RemoveRange(delmapPM);
                    permission.MapPermissionMenu = mapPM;

                    returnMsg = "編輯成功";
                }
                else
                {
                    var _tempPermissionName = UpdateAdminGroup.CodeName.Trim().ToUpper();
                    if (db.TblPermission.Where(a => a.CodeName.ToUpper() == _tempPermissionName).FirstOrDefault() != null)
                    {
                        result = new APIReturn(APIReturnCode.Fail, "相同權限名稱已存在");
                        return result;
                    }


                    //新增
                    db.TblPermission.Add(new TblPermission()
                    {
                        CodeName = UpdateAdminGroup.CodeName,
                        IsEnable = UpdateAdminGroup.IsEnable,
                        MapPermissionMenu = mapPM
                    });
                    returnMsg = "新增成功";
                }

                //寫入 DB
                db.SaveChanges();

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
        /// 傳入 ID 刪除指定權限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APIReturn DeleteAdminPermission(int ID)
        {
            try
            {
                //找出該權限
                TblPermission permission = db.TblPermission.Find(ID);

                //刪除
                db.TblPermission.Remove(permission);

                //寫入 DB
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
