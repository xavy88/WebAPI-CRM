using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Models.Dtos
{
    public class PositionDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public virtual DepartmentDto Department { get; set; }
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }
    }
}
