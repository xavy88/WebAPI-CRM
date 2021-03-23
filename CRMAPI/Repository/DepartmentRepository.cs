using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _db;

        public DepartmentRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActiveDepartment(Department department)
        {
            _db.Departments.Update(department);
            return Save();
        }

        public bool CreateDepartment(Department department)
        {
            _db.Departments.Add(department);
            return Save();
        }

        public bool DeleteDepartment(Department department)
        {
            _db.Departments.Remove(department);
            return Save();
        }

        public bool DepartmentExists(string name)
        {
            bool value = _db.Departments.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool DepartmentExists(int id)
        {
            return _db.Departments.Any(a => a.Id == id);
        }

        public Department GetDepartment(int departmentId)
        {
            return _db.Departments.FirstOrDefault(a => a.Id == departmentId);
        }

        public ICollection<Department> GetDepartments()
        {
            return _db.Departments.OrderBy(a => a.Name).ToList();
        }

        public bool InactiveDepartment(Department department)
        {
            _db.Departments.Update(department);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateDepartment(Department department)
        {
            _db.Departments.Update(department);
            return Save();
        }
    }
}
