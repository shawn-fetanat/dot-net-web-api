using Sabio.Models;
using Sabio.Models.Domain.codingChallenge;
using Sabio.Models.Requests;
using Sabio.Models.Requests.CodingChallenge;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sabio.Services
{
    public interface ICourseService
    {
        Course Get(int id);

        int Add(CourseAddRequest model);

        void Update(CourseUpdateRequest model);

        Paged<Course> GetPage(int pageIndex, int pageSize);
    }
}
