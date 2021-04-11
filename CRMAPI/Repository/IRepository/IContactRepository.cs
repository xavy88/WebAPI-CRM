using CRMAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMAPI.Repository.IRepository
{
    public interface IContactRepository
    {
        ICollection<Contact> GetContacts();
        ICollection<Contact> GetContactsInAccount(int accountId);
        Contact GetContact(int contactId);
        bool ContactExists(string name);
        bool ContactExists(int id);
        bool CreateContact(Contact contact);
        bool UpdateContact(Contact contact);
        bool DeleteContact(Contact contact);
        bool Save();
    }
}

