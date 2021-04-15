using CRMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository.IRepository
{
    public interface ITaskAssignmentRepository
    {
        ICollection<TaskAssignment> GetTaskAssignments();
        ICollection<TaskAssignment> GetTaskAssignmentInUser(int userId);
        TaskAssignment GetTaskAssignment(int taskAssignmentId);
       // bool TaskAssignmentExists(string name);
        bool TaskAssignmentExists(int id);
        bool CreateTaskAssignment(TaskAssignment taskAssignment);
        bool UpdateTaskAssignment(TaskAssignment taskAssignment);
        bool InactiveTaskAssignment(TaskAssignment taskAssignment);
        bool ActiveTaskAssignment(TaskAssignment taskAssignment);
        bool DeleteTaskAssignment(TaskAssignment taskAssignment);
        bool Save();
    }
}

