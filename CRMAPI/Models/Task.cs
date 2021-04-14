using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Models
{
    public class Task
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Duration { get; set; }
        public string Description { get; set; }
        [Display(Name = "Position")]
        public int PositionId { get; set; }
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        public bool IsActive { get; set; }
    }
}
