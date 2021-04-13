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
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _db;

        public ServiceRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActiveService(Service service)
        {
            _db.Services.Update(service);
            return Save();
        }

        public bool CreateService(Service service)
        {
            _db.Services.Add(service);
            return Save();
        }

        public bool DeleteService(Service service)
        {
            _db.Services.Remove(service);
            return Save();
        }

        public bool ServiceExists(string name)
        {
            bool value = _db.Services.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool ServiceExists(int id)
        {
            return _db.Services.Any(a => a.Id == id);
        }

        public Service GetService(int serviceId)
        {
            return _db.Services.Include(c => c.Department).FirstOrDefault(a => a.Id == serviceId);
        }

        public ICollection<Service> GetServices()
        {
            return _db.Services.Include(c => c.Department).OrderBy(a => a.Name).ToList();
        }

        public bool InactiveService(Service service)
        {
            _db.Services.Update(service);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateService(Service service)
        {
            _db.Services.Update(service);
            return Save();
        }

        public ICollection<Service> GetServicesInDepartment(int departmentId)
        {
            return _db.Services.Include(c => c.Department)
                .Where(c => c.DepartmentId == departmentId).ToList();

        }
    }
}
