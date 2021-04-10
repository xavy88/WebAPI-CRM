using CRMWeb.Models;
using CRMWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRMWeb.Repository
{
    public class ClientRepository : Repository<Account>, IClientRepository 
    {
        private readonly IHttpClientFactory _clientFactory;

        public ClientRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

    }
}
