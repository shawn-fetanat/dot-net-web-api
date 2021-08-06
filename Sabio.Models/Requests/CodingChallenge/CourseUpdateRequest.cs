using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Requests.CodingChallenge
{
    public class CourseUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int SeasonTermId { get; set; }

        public int TeacherId { get; set; }
    }
}
