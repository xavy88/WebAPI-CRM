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
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActiveTask(Models.Task task)
        {
            _db.Tasks.Update(task);
            return Save();
        }

        public bool CreateTask(Models.Task task)
        {
            _db.Tasks.Add(task);
            return Save();
        }

        public bool DeleteTask(Models.Task task)
        {
            _db.Tasks.Remove(task);
            return Save();
        }

        public bool TaskExists(string name)
        {
            bool value = _db.Tasks.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TaskExists(int id)
        {
            return _db.Tasks.Any(a => a.Id == id);
        }

        public Models.Task GetTask(int taskId)
        {
            return _db.Tasks.Include(c => c.Department).FirstOrDefault(a => a.Id == taskId);
        }

        public ICollection<Models.Task> GetTasks()
        {
            return _db.Tasks.Include(c => c.Department).Include(p=>p.Position).OrderBy(a => a.Name).ToList();
        }

        public bool InactiveTask(Models.Task task)
        {
            _db.Tasks.Update(task);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTask(Models.Task task)
        {
            _db.Tasks.Update(task);
            return Save();
        }

        public ICollection<Models.Task> GetTaskInDepartment(int departmentId)
        {
            return _db.Tasks.Include(c => c.Department)
                .Where(c => c.DepartmentId == departmentId).ToList();

        }
    }
}
