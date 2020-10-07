using System;
using System.Collections.Generic;

namespace angular_API.Model.EFModel
{
    public partial class MapPermissionMenu
    {
        public int PermissionId { get; set; }
        public int MenuId { get; set; }

        public TblMenu Menu { get; set; }
        public TblPermission Permission { get; set; }
    }
}
