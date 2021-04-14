using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Models.ViewModel
{
    public class ServiceVM
    {
        public IEnumerable<SelectListItem> DepartmentList { get; set; }

        public Service Service { get; set; }
    }
}
