using System;
using System.Collections.Generic;

namespace angular_API.Model.EFModel
{
    public partial class TblOperationLog
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Parameter { get; set; }
        public DateTime RequestTime { get; set; }
        public string Ip { get; set; }
        public int Operator { get; set; }
        public string Remark { get; set; }

        public TblAdmin OperatorNavigation { get; set; }
    }
}
