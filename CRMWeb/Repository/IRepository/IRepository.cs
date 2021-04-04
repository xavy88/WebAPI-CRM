﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        Task<T> GetAsync(string url, int Id, string token);
        Task<IEnumerable<T>> GetAllAsync(string url, string token);
        Task<bool> CreateAsync(string url, T objCreate, string token);
        Task<bool> UpdateAsync(string url, T objUpdate, string token);
        Task<bool> DeleteAsync(string url, int Id, string token);

    }
}
