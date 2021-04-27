using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository
{
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskAssignmentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActiveTaskAssignment(TaskAssignment taskAssignment)
        {
            _db.TaskAssignments.Update(taskAssignment);
            return Save();
        }

        public bool CreateTaskAssignment(TaskAssignment taskAssignment)
        {
            _db.TaskAssignments.Add(taskAssignment);
            return Save();
        }

        public bool DeleteTaskAssignment(TaskAssignment taskAssignment)
        {
            _db.TaskAssignments.Remove(taskAssignment);
            return Save();
        }

        public bool TaskAssignmentExists(int id)
        {
            return _db.TaskAssignments.Any(a => a.Id == id);
        }

        public TaskAssignment GetTaskAssignment(int taskAssignmentId)
        {
            return _db.TaskAssignments.Include(c => c.Department).Include(d => d.Account).Include(u => u.Employee).Include(t => t.Task).FirstOrDefault(a => a.Id == taskAssignmentId);
        }

        public ICollection<TaskAssignment> GetTaskAssignments()
        {
            return _db.TaskAssignments.Include(c => c.Department).Include(d => d.Account).Include(u => u.Employee).Include(t => t.Task).OrderBy(a => a.DueDate).ToList();
        }

        public bool InactiveTaskAssignment(TaskAssignment taskAssignment)
        {
            _db.TaskAssignments.Update(taskAssignment);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTaskAssignment(TaskAssignment taskAssignment)
        {
            _db.TaskAssignments.Update(taskAssignment);
            return Save();
        }

        public ICollection<TaskAssignment> GetTaskAssignmentInUser(int userId)
        {
            return _db.TaskAssignments.Include(c => c.Employee)
                .Where(c => c.Id == userId).ToList();

        }
    }
}
