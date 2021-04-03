using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public byte[] Picture { get; set; }
        public string Token { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
    }
}
