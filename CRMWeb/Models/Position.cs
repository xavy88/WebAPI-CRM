using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Models
{
    public class Position
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Department")]
        [Required]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

    }
}
