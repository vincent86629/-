using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;


namespace angular_API.Model.PageModel.Admin.AdminManage
{
    public class AdminLogRequestModel
    {
        // 開始時間
        public DateTime? StartDate { get; set; }
        // 結束時間
        public DateTime? EndDate { get; set; }
        // 操作者
        public string Operator { get; set; }
        // 事件
        public bool SelectLogin { get; set; }
        public bool SelectGiftLog { get; set; }
        public bool SelectExpertApprove { get; set; }
        public bool SelectLogout { get; set; }
        public bool SelectExpertManage { get; set; }
        public bool SelectQAlog { get; set; }

        //IP
        public string Ip { get; set; }
        // 備註
        public string Remark { get; set; }


        public AdminLogRequestModel()
        {
           
        }
    }

    public class AdminLogResponseModel
    {
        // ID
        public int Id { get; set; }
        // 事件
        public string Event { get; set; }
        // 操作者
        public string Operator { get; set; }
        // 時間
        public DateTime RequestTime { get; set; }
        //IP
        public string Ip { get; set; }
        // 備註
        public string Remark { get; set; }

        public AdminLogResponseModel()
        {

        }
    }

    public class AdminLog
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
    }

    public enum AdminLogType
    {
        [Description("Login")]
        Login,
        [Description("Logout")]
        Logout,
        [Description("GiftLog")]
        GiftLog,
        [Description("ExpertManage")]
        ExpertManage,
        [Description("ExpertApprove")]
        ExpertApprove,
        [Description("QALog")]
        QALog
    }




}
