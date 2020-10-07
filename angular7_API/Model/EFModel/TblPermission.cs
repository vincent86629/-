using System;
using System.Collections.Generic;

namespace angular_API.Model.EFModel
{
    public partial class TblPermission
    {
        public TblPermission()
        {
            MapPermissionMenu = new HashSet<MapPermissionMenu>();
            MapUserPermission = new HashSet<MapUserPermission>();
        }

        public int Id { get; set; }
        public bool IsEnable { get; set; }
        public string CodeName { get; set; }

        public ICollection<MapPermissionMenu> MapPermissionMenu { get; set; }
        public ICollection<MapUserPermission> MapUserPermission { get; set; }
    }
}
