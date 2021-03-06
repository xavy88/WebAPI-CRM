using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Hired Date")]
        public DateTime HiredDate { get; set; }
        public byte[] Image { get; set; }
        [Display(Name = "Position")]
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public bool IsActive { get; set; }
    }
}
