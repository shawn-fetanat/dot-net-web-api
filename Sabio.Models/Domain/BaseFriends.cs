using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Domain
{
    public class BaseFriends
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }

        public string StatusId { get; set; }
    }
}
