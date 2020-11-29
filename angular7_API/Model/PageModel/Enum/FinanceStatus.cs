using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace angular_API.Model.PageModel.Enum
{
    public enum FinanceStatus
    {
        [Description("草稿")]
        Draft = 0,
        [Description("完成")]
        Complete = 1,
        [Description("作廢")]
        Invalid = 2
    }
}
