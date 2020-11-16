using System;
using System.Collections.Generic;

namespace angular_API.Model.EFModel
{
    public partial class TblReportFile
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public bool IsDelete { get; set; }

        public TblReport Report { get; set; }
    }
}
