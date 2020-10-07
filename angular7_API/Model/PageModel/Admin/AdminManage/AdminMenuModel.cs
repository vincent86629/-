using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace angular_API.Model.PageModel.Admin.AdminManage
{
    public class AdminMenuModel
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public List<AdminMenuModel> Childs { get; set; }

        public AdminMenuModel()
        {
            Childs = new List<AdminMenuModel>();
        }
    }
}
