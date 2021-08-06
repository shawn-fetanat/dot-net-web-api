using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Requests
{
    public class CourseAddRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int SeasonTermId { get; set; }

        public int TeacherId { get; set; }
    }
}
