using System;
using System.Linq.Expressions;
using ContactManager.Access.Data;
using ContactManager.Access.Models;

namespace ContactManager.Access.Service.IService
{
    public interface IContactService
    {
        Task DeleteContact(Guid id);
        Task<IEnumerable<Contact>> GetAllContacts(string? includeProperties = null);
        Task<Contact?> GetContact(Expression<Func<Contact, bool>> filter, string? includeProperties = null);
        Task<Contact> SaveContact(SaveContactViewModel model);
    }
}
