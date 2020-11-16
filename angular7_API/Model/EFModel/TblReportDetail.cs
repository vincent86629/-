using System;
using System.Collections.Generic;

namespace angular_API.Model.EFModel
{
    public partial class TblReportDetail
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string BlockName { get; set; }
        public string Rows { get; set; }
        public string TotalName { get; set; }
        public int? Total { get; set; }

        public TblReport Report { get; set; }
    }
}
