using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMWeb.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        Task<T> GetAsync(string url, int Id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<bool> CreateAsync(string url, T objCreate);
        Task<bool> UpdateAsync(string url, T objUpdate);
        Task<bool> DeleteAsync(string url, int Id);

    }
}
