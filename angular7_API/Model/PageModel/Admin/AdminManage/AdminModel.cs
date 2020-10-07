using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace angular_API.Model.PageModel.Admin.AdminManage
{
    public class AdminModel
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EmployeeId { get; set; }
        public bool IsEnable { get; set; }
        public string Name { get; set; }

        public List<AdminGroupModel> Groups { get; set; }
        public string GroupsName { get; set; }

        public AdminModel()
        {
            Groups = new List<AdminGroupModel>();
        }
    }
}
