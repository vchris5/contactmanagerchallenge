using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManager.Access.Data
{
    public class EmailAddress : Entity
    {
        public string Email { get; set; }
        public EmailType Type { get; set; }

        /// <summary>
        /// Gets or sets whether the email address is the primary email for the associated contact.
        /// </summary>
        [DefaultValue(false)]
        public bool IsPrimary { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the unique identifier of the associated contact.
        /// </summary>
        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
