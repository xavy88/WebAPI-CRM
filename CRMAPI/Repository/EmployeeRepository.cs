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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActiveEmployee(Employee employee)
        {
            _db.Employees.Update(employee);
            return Save();
        }

        public bool CreateEmployee(Employee employee)
        {
            _db.Employees.Add(employee);
            return Save();
        }

        public bool DeleteEmployee(Employee employee)
        {
            _db.Employees.Remove(employee);
            return Save();
        }

        public bool EmployeeExists(string name)
        {
            bool value = _db.Employees.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool EmployeeExists(int id)
        {
            return _db.Employees.Any(a => a.Id == id);
        }

        public Employee GetEmployee(int employeeId)
        {
            return _db.Employees.Include(c => c.Department).FirstOrDefault(a => a.Id == employeeId);
        }

        public ICollection<Employee> GetEmployees()
        {
            return _db.Employees.Include(c => c.Department).OrderBy(a => a.Name).ToList();
        }

        public bool InactiveEmployee(Employee employee)
        {
            _db.Employees.Update(employee);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateEmployee(Employee employee)
        {
            _db.Employees.Update(employee);
            return Save();
        }

        public ICollection<Employee> GetEmployeesInDepartment(int departmentId)
        {
            return _db.Employees.Include(c => c.Department)
                .Where(c => c.DepartmentId == departmentId).ToList();

        }
    }
}
