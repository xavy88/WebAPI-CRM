using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Models.ViewModel
{
    public class EmployeeVM
    {
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> PositionList { get; set; }
        public Employee Employee { get; set; }
    }
}
