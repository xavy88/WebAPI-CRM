using CRMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository.IRepository
{
    public interface IAccountRepository
    {
        ICollection<Account> GetAccounts();
        Account GetAccount(int accountId);
        bool AccountExists(string name);
        bool AccountExists(int id);
        bool CreateAccount(Account account);
        bool UpdateAccount(Account account);
        bool InactiveAccount(Account account);
        bool ActiveAccount(Account account);
        bool DeleteAccount(Account account);
        bool Save();
    }
}

