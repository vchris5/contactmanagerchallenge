using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactManager.Access.Data;

namespace ContactManager.Access.Models
{
    public class EmailViewModel
    {
        public EmailType Type { get; set; }
        public string Email { get; set; }
        
        // Gets or sets a boolean indicating whether the email is primary.
        public bool IsPrimary { get; set; }
    }
}
