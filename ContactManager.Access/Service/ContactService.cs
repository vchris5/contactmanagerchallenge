using System;
using System.Linq.Expressions;
using ContactManager.Access.Data;
using ContactManager.Access.Models;
using ContactManager.Access.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using ContactManager.Access.Service.IService;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ContactManager.Access.Service
{
    /// <summary>
    /// Service class responsible for handling contact-related business logic.
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ContactService> _logger;

        // Constructor to initialize the service with required dependencies.
        public ContactService(IUnitOfWork unitOfWork, ILogger<ContactService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // Method to delete a contact by ID.
        public async Task DeleteContact(Guid id)
        {
            try
            {
                LogInformation($"Attempting to delete contact {id}.");
                var contact = await _unitOfWork.Contacts.Get(u => u.Id == id, "EmailAddresses");
                if (contact != null)
                {
                    _unitOfWork.Addresses.RemoveRange(contact.Addresses);
                    _unitOfWork.Emails.RemoveRange(contact.EmailAddresses);
                    _unitOfWork.Contacts.Remove(contact);
                    LogInformation($"Attempting to save delete operation on {id}.");
                    await _unitOfWork.Save();
                    LogInformation($"Successfully saved delete operation on {id}.");
                }
                else
                {
                    LogInformation($"Could not find contact with ID {id}");
                }
            }
            catch (Exception e)
            {
                HandleServiceError($"Error occurred while deleting contact {id}.", e);
                throw;
            }
        }

        // Method to retrieve all contacts with optional inclusion of related properties.
        public async Task<IEnumerable<Contact>> GetAllContacts(string? includeProperties = null)
        {
            try
            {
                LogInformation($"Attempting to retrieve all contacts.");
                var contacts = await _unitOfWork.Contacts.GetAll(includeProperties);
                return contacts;
            }
            catch (Exception e)
            {
                HandleServiceError($"Error occurred while retrieving all contacts.", e);
                throw;
            }
        }

        // Method to retrieve a contact based on filter criteria with optional inclusion of related properties.
        public async Task<Contact?> GetContact(Expression<Func<Contact, bool>> filter, string? includeProperties = null)
        {
            try
            {
                LogInformation($"Attempting to retrieve contact.");
                var contact = (await _unitOfWork.Contacts.Get(filter, includeProperties));
                LogInformation("Returning contact matching criteria.");
                return contact;
            }
            catch (Exception e)
            {
                HandleServiceError("Error occurred while retrieving contact matching criteria.", e);
                throw;
            }
        }

        // Method to save or update a contact based on the provided model.
        public async Task<Contact> SaveContact(SaveContactViewModel model)
        {
            LogInformation($"Attempting to save contact. {model.ContactId}");

            List<EmailViewModel> primaryEmails = model.Emails.FindAll(u => u.IsPrimary == true);
            if (primaryEmails.Count > 1)
            {
                HandleServiceError("Error occurred, contact provided has more than 1 primary email, cannot save.", null);
                throw new Exception("Duplicate primary emails provided.");
            }

            bool newContact = model.ContactId == Guid.Empty;
            var contact = newContact
                ? new Contact { Title = model.Title, FirstName = model.FirstName, LastName = model.LastName, DOB = model.DOB }
                : await _unitOfWork.Contacts.Get(x => x.Id == model.ContactId, "EmailAddresses,Addresses");

            if (contact == null)
            {
                HandleServiceError("Cannot save empty contact.", null);
                throw new Exception("Failed to create a new contact with the given model");
            }

            LogInformation($"Removing contact email and addresses from tables.");
            _unitOfWork.Emails.RemoveRange(contact.EmailAddresses);
            _unitOfWork.Addresses.RemoveRange(contact.Addresses);

            foreach (var email in model.Emails)
            {
                contact.EmailAddresses.Add(new EmailAddress
                {
                    Type = email.Type,
                    Email = email.Email,
                    IsPrimary = email.IsPrimary,
                    Contact = contact,
                    ContactId = contact.Id
                });
            }

            foreach (var address in model.Addresses)
            {
                contact.Addresses.Add(new Address
                {
                    Street1 = address.Street1,
                    Street2 = address.Street2,
                    City = address.City,
                    State = address.State,
                    Zip = address.Zip,
                    Type = address.Type,
                    Contact = contact
                });
            }

            contact.Title = model.Title;
            contact.FirstName = model.FirstName;
            contact.LastName = model.LastName;
            contact.DOB = model.DOB;

            LogInformation($"New contact created.");
            LogInformation($"Attempting to upsert contact. {contact.Id}");

            try
            {
                if (newContact)
                {
                    await _unitOfWork.Contacts.Add(contact);
                }
                else
                {
                    _unitOfWork.Contacts.Update(contact);
                }

                LogInformation($"Upsert performed.");
                LogInformation($"Attempting to save operations performed");

                await _unitOfWork.Save();

                LogInformation($"Successfully saved contact {contact.Id} in the database. ");

                return contact;
            }

            catch (Exception e)
            {
                HandleServiceError($"Error occurred while saving contact to the database.", e);
                throw;
            }
        }

        // Private method to handle logging information.
        private void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        // Private method to handle service-level errors and log them.
        private void HandleServiceError(string message, Exception? exception)
        {
            _logger.LogError(exception, message);
        }
    }
}
