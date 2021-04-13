using CRMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository.IRepository
{
    public interface IServiceRepository
    {
        ICollection<Service> GetServices();
        ICollection<Service> GetServicesInDepartment(int departmentId);
        Service GetService(int serviceId);
        bool ServiceExists(string name);
        bool ServiceExists(int id);
        bool CreateService(Service service);
        bool UpdateService(Service service);
        bool InactiveService(Service service);
        bool ActiveService(Service service);
        bool DeleteService(Service service);
        bool Save();
    }
}

