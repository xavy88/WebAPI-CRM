using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Models
{
    public class TaskAssignment
    {
        public TaskAssignment()
        {
            StartDate = DateTime.Now;
        }
        public int Id { get; set; }
        [Display(Name = "Account")]
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
        [Display(Name = "Task")]
        public int TaskId { get; set; }
        [ForeignKey("TaskId")]
        public virtual Models.Task Task { get; set; }
        [Display(Name = "Assignee")]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        public string Notes { get; set; }
        public bool Completed { get; set; }
    }
}
