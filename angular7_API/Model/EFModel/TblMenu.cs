using System;
using System.Collections.Generic;

namespace angular_API.Model.EFModel
{
    public partial class TblMenu
    {
        public TblMenu()
        {
            MapPermissionMenu = new HashSet<MapPermissionMenu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int ParentId { get; set; }
        public short Seq { get; set; }
        public bool IsEnable { get; set; }

        public ICollection<MapPermissionMenu> MapPermissionMenu { get; set; }
    }
}
