using System;
using System.Collections.Generic;

namespace angular_API.Model.EFModel
{
    public partial class MapUserPermission
    {
        public int AdminId { get; set; }
        public int PermissionId { get; set; }

        public TblAdmin Admin { get; set; }
        public TblPermission Permission { get; set; }
    }
}
