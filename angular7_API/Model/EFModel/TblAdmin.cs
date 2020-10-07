using System;
using System.Collections.Generic;

namespace angular_API.Model.EFModel
{
    public partial class TblAdmin
    {
        public TblAdmin()
        {
            MapUserPermission = new HashSet<MapUserPermission>();
            TblOperationLog = new HashSet<TblOperationLog>();
        }

        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EmployeeId { get; set; }
        public bool IsEnable { get; set; }
        public bool IsPasswordConfirmed { get; set; }

        public ICollection<MapUserPermission> MapUserPermission { get; set; }
        public ICollection<TblOperationLog> TblOperationLog { get; set; }
    }
}
