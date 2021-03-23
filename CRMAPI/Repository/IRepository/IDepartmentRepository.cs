using CRMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository.IRepository
{
    public interface IDepartmentRepository
    {
        ICollection<Department> GetDepartments();
        Department GetDepartment(int departmentId);
        bool DepartmentExists(string name);
        bool DepartmentExists(int id);
        bool CreateDepartment(Department department);
        bool UpdateDepartment(Department department);
        bool InactiveDepartment(Department department);
        bool ActiveDepartment(Department department);
        bool DeleteDepartment(Department department);
        bool Save();
    }
}

