using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Models.Dtos
{
    public class EmployeeUpdateDto
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
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; }
    }
}
