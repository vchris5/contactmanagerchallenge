using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactManager.Access.Data;
using ContactManager.Hubs;
using ContactManager.Access.Models;
using ContactManager.Access.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IHubContext<ContactHub> _hubContext;
        private readonly ILogger<ContactsController> _logger;

        // Constructor with dependency injection
        public ContactsController(IContactService contactService, IHubContext<ContactHub> hubContext, ILogger<ContactsController> logger)
        {
            _contactService = contactService;
            _hubContext = hubContext;
            _logger = logger;
        }

        // Action for deleting a contact
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            try
            {
                await _contactService.DeleteContact(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred after the request to delete contact {id}");
                return BadRequest();
            }

            await _hubContext.Clients.All.SendAsync("Update");

            return Ok();
        }

        // Action for editing a contact
        public async Task<IActionResult> EditContact(Guid id)
        {
            try
            {
                var contact = await _contactService.GetContact(u => u.Id == id, "EmailAddresses,Addresses");
                if (contact == null)
                {
                    return NotFound();
                }

                var viewModel = new EditContactViewModel
                {
                    Id = contact.Id,
                    Title = contact.Title,
                    FirstName = contact.FirstName,
                    LastName = contact.LastName,
                    DOB = contact.DOB,
                    EmailAddresses = contact.EmailAddresses,
                    Addresses = contact.Addresses
                };

                return PartialView("_EditContact", viewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred while retrieving contact {id} for an edit request.");
                return BadRequest();
            }
        }

        // Action for retrieving all contacts
        public async Task<IActionResult> GetContacts()
        {
            try
            {
                var contactList = (await _contactService.GetAllContacts("EmailAddresses")).OrderBy(u => u.FirstName).ToList();
                return PartialView("_ContactTable", new ContactViewModel { Contacts = contactList });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error occurred while retrieving all contacts.");
                return NotFound();
            }
        }

        // Action for the index view
        public IActionResult Index()
        {
            return View();
        }

        // Action for creating a new contact
        public IActionResult NewContact()
        {
            return PartialView("_EditContact", new EditContactViewModel());
        }

        // Action for saving a contact
        [HttpPost]
        public async Task<IActionResult> SaveContact([FromBody] SaveContactViewModel model)
        {
            try
            {
                var savedContact = await _contactService.SaveContact(model);
                if (savedContact == null)
                {
                    return NotFound();
                }

                await _hubContext.Clients.All.SendAsync("Update");

                SendEmailNotification(savedContact.Id);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error while attempting to save model with ID {model.ContactId}");
                return BadRequest();
            }
        }

        // Method for sending email notification
        private void SendEmailNotification(Guid contactId)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("noreply", "noreply@contactmanager.com"));
            message.To.Add(new MailboxAddress("SysAdmin", "Admin@contactmanager.com"));
            message.Subject = "ContactManager System Alert";

            message.Body = new TextPart("plain")
            {
                Text = "Contact with id:" + contactId.ToString() + " was updated"
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("127.0.0.1", 25, false);
                    client.Send(message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Error occurred while sending contact update email about contact {contactId}");
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }
    }
}