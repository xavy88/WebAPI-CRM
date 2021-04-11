using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Models.Dtos
{
    public class ContactUpdateDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(30)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Position { get; set; }
        public int AccountId { get; set; }
        public string Description { get; set; }
    }
}
