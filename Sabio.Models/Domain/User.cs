using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Domain
{
    public class User : BaseUsers
    {
        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PasswordConfirm { get; set; }

        public string AvatarUrl { get; set; }

    }
}
