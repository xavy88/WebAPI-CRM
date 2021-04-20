using CRMWeb.Models;
using CRMWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRMWeb.Repository     
{
    public class TaskAssignmentRepository : Repository<TaskAssignment>, ITaskAssignmentRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public TaskAssignmentRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

    }
}
