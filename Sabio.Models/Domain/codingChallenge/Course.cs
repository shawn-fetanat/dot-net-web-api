using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Models.Domain.codingChallenge
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SeasonTerm { get; set; }

        public string Teacher { get; set; }

        public IList<Student> Student { get; set; }

    }
}
