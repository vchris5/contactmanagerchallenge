using System;
using System.Threading.Tasks;
using ContactManager.Access.Data;
using ContactManager.Access.Repository.IRepository;

namespace ContactManager.Access.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _db;
        public IRepository<Contact> Contacts { get; private set; }
        public IRepository<Address> Addresses { get; private set; }
        public IRepository<EmailAddress> Emails { get; private set; }

        public UnitOfWork(ApplicationContext db)
        {
            _db = db;

            // Initialize repositories with the appropriate DbSet
            Contacts = new Repository<Contact>(_db.Contacts);
            Addresses = new Repository<Address>(_db.Addresses);
            Emails = new Repository<EmailAddress>(_db.EmailAddresses);
        }

        /// <summary>
        /// Saves changes made in the current unit of work to the underlying database.
        /// </summary>
        /// <returns>An asynchronous task representing the process of saving changes.</returns>
        public async Task Save()
        {
            // Save changes to the database
            await _db.SaveChangesAsync();
        }
    }
}
