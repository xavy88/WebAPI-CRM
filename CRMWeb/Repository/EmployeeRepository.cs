using CRMWeb.Models;
using CRMWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRMWeb.Repository     
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public EmployeeRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

    }
}
