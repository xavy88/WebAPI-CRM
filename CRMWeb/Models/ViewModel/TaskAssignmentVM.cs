using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Models.ViewModel
{
    public class TaskAssignmentVM
    {
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }
        public IEnumerable<SelectListItem> TaskList { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }
        public TaskAssignment TaskAssignment { get; set; }
    }
}
