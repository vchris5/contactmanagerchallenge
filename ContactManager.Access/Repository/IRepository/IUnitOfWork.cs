using System;
using System.Threading.Tasks;
using ContactManager.Access.Data;

namespace ContactManager.Access.Repository.IRepository
{
    /// <summary>
    /// Interface for Unit of Work pattern to manage repositories and save changes to the database.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets the repository for managing contacts.
        /// </summary>
        public IRepository<Contact> Contacts { get; }

        /// <summary>
        /// Gets the repository for managing email addresses.
        /// </summary>
        public IRepository<EmailAddress> Emails { get; }

        /// <summary>
        /// Gets the repository for managing addresses.
        /// </summary>
        public IRepository<Address> Addresses { get; }

        /// <summary>
        /// Asynchronously saves the changes made in the current unit of work to the underlying database.
        /// </summary>
        /// <returns>An asynchronous task representing the process of saving changes.</returns>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">An error occurred while updating the database.</exception>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">A concurrency error occurred while updating the database.</exception>
        /// <exception cref="OperationCanceledException">An error occurred due to a canceled operation.</exception>
        Task Save();
    }
}
