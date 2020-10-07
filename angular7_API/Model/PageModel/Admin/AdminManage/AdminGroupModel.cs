using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace angular_API.Model.PageModel.Admin.AdminManage
{
    //權限
    public class AdminGroupModel
    {
        public int Id { get; set; }
        public bool IsEnable { get; set; }
        public string CodeName { get; set; }
        public bool IsChecked { get; set; }
        public List<PermissionMenuModel> permissionMenus { get; set; }  //DB撈出來的tree
        public int[] CheckedMenus { get; set; } //要存進DB的ID
    }

    //權限tree
    public class PermissionMenuModel
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public List<PermissionMenuModel> Child { get; set; }

        public PermissionMenuModel() {
            Child = new List<PermissionMenuModel>();
         }
    }

    //
    public class PermissionMenuFilter
    {
        public int PermissionId { get; set; }
        public int ParentId { get; set; }
    }


}
