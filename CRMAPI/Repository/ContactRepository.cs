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
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _db;

        public ContactRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateContact(Contact contact)
        {
            _db.Contacts.Add(contact);
            return Save();
        }

        public bool DeleteContact(Contact contact)
        {
            _db.Contacts.Remove(contact);
            return Save();
        }

        public bool ContactExists(string name)
        {
            bool value = _db.Contacts.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool ContactExists(int id)
        {
            return _db.Contacts.Any(a => a.Id == id);
        }

        public Contact GetContact(int contactId)
        {
            return _db.Contacts.Include(c => c.Account).FirstOrDefault(a => a.Id == contactId);
        }

        public ICollection<Contact> GetContacts()
        {
            return _db.Contacts.Include(c => c.Account).OrderBy(a => a.Name).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateContact(Contact contact)
        {
            _db.Contacts.Update(contact);
            return Save();
        }

        public ICollection<Contact> GetContactsInAccount(int accountId)
        {
            return _db.Contacts.Include(c => c.Account)
                .Where(c => c.AccountId == accountId).ToList();

        }
    }
}
