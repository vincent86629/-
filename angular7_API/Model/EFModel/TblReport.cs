using System;
using System.Collections.Generic;

namespace angular_API.Model.EFModel
{
    public partial class TblReport
    {
        public TblReport()
        {
            TblReportDetail = new HashSet<TblReportDetail>();
            TblReportFile = new HashSet<TblReportFile>();
        }

        public int Id { get; set; }
        public string YearMonth { get; set; }
        public int PermissionId { get; set; }
        public int CreateBy { get; set; }
        public int LastMonthBalance { get; set; }
        public int ThisMonthBalance { get; set; }
        public string BankSaving { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Status { get; set; }

        public TblAdmin CreateByNavigation { get; set; }
        public TblPermission Permission { get; set; }
        public ICollection<TblReportDetail> TblReportDetail { get; set; }
        public ICollection<TblReportFile> TblReportFile { get; set; }
    }
}
