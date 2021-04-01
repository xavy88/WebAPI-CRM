using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Models.ViewModel
{
    public class IndexVM
    {
        public IEnumerable<Department> DepartmentList { get; set; }
        public IEnumerable<Position> PositionList { get; set; }
    }
}
