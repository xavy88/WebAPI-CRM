using CRMAPI.Data;
using CRMAPI.Models;
using CRMAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;

        public AccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ActiveAccount(Account account)
        {
            _db.Accounts.Update(account);
            return Save();
        }

        public bool CreateAccount(Account account)
        {
            _db.Accounts.Add(account);
            return Save();
        }

        public bool DeleteAccount(Account account)
        {
            _db.Accounts.Remove(account);
            return Save();
        }

        public bool AccountExists(string name)
        {
            bool value = _db.Accounts.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool AccountExists(int id)
        {
            return _db.Accounts.Any(a => a.Id == id);
        }

        public Account GetAccount(int accountId)
        {
            return _db.Accounts.FirstOrDefault(a => a.Id == accountId);
        }

        public ICollection<Account> GetAccounts()
        {
            return _db.Accounts.OrderBy(a => a.Name).ToList();
        }

        public bool InactiveAccount(Account account)
        {
            _db.Accounts.Update(account);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateAccount(Account account)
        {
            _db.Accounts.Update(account);
            return Save();
        }
    }
}
