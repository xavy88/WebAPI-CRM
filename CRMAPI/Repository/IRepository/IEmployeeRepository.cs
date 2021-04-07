using CRMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        ICollection<Employee> GetEmployees();
        ICollection<Employee> GetEmployeesInDepartment(int departmentId);
        Employee GetEmployee(int employeeId);
        bool EmployeeExists(string name);
        bool EmployeeExists(int id);
        bool CreateEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);
        bool InactiveEmployee(Employee employee);
        bool ActiveEmployee(Employee employee);
        bool DeleteEmployee(Employee employee);
        bool Save();
    }
}

