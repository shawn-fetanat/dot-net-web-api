using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Domain
{
    public class Friend : BaseFriends
    {
        public string Title { get; set; }

        public string Bio { get; set; }

        public string Summary { get; set; }

        public string Headline { get; set; }

        public string Slug { get; set; }

        public string Skills { get; set; }

        public string PrimaryImage { get; set; }

    }
}
