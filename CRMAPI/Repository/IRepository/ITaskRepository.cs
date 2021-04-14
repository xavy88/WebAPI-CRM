using CRMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository.IRepository
{
    public interface ITaskRepository
    {
        ICollection<Models.Task> GetTasks();
        ICollection<Models.Task> GetTaskInDepartment(int departmentId);
        Models.Task GetTask(int taskId);
        bool TaskExists(string task);
        bool TaskExists(int id);
        bool CreateTask(Models.Task task);
        bool UpdateTask(Models.Task task);
        bool InactiveTask(Models.Task task);
        bool ActiveTask(Models.Task task);
        bool DeleteTask(Models.Task task);
        bool Save();
    }
}

