using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Models.ViewModel
{
    public class PositionVM
    {
        public IEnumerable<SelectListItem> DepartmentList { get; set; }

        public Position Position { get; set; }
    }
}
