using Newtonsoft.Json;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.codingChallenge;
using Sabio.Models.Requests;
using Sabio.Models.Requests.CodingChallenge;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services.codingChallenge
{
    public class CourseService: ICourseService
    {
        IDataProvider _data = null;

        public CourseService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(CourseAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[Courses_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Name", model.Name);
                col.AddWithValue("@Description", model.Description);
                col.AddWithValue("@SeasonTermId", model.SeasonTermId);
                col.AddWithValue("@TeacherId", model.TeacherId);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {

                object oId = returnCollection["@Id"].Value;
                Int32.TryParse(oId.ToString(), out id);

            });
            return id;
        }

        public Course Get(int id)
        {

            Course course = new Course();

            string procName = "[dbo].[Courses_SelectById]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {

                parameterCollection.AddWithValue("@Id", id);


            }, delegate (IDataReader reader, short set)
            {
                course = MapCourse(reader);
            }
            );
            return course;
        }

        public Paged<Course> GetPage(int pageIndex, int pageSize)
        {
            Paged<Course> pagedList = null;

            List<Course> list = null;

            int totalCount = 0;

            string procName = "[dbo].[Courses_SelectPaginated]";

            _data.ExecuteCmd(procName, (param) =>
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);

            }, (reader, recordSetIndex) =>
            {

                Course aCourse = MapCourse(reader);
                totalCount = reader.GetSafeInt32(6);


                if (list == null)
                {
                    list = new List<Course>();
                }

                list.Add(aCourse);
            }
            );
            if (list != null)
            {
                pagedList = new Paged<Course>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public void Update(CourseUpdateRequest model)
        {

            string procName = "[dbo].[Courses_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                
                col.AddWithValue("@Name", model.Name);
                col.AddWithValue("@Description", model.Description);
                col.AddWithValue("@SeasonTermId", model.SeasonTermId);
                col.AddWithValue("@TeacherId", model.TeacherId);
                col.AddWithValue("@Id", model.Id);


            }, returnParameters: null);

        }

        private static Course MapCourse(IDataReader reader)
        {
            Course course = new Course();

            int i = 0;

            course.Id = reader.GetSafeInt32(i++);
            course.Name = reader.GetSafeString(i++);
            course.Description = reader.GetSafeString(i++);
            course.SeasonTerm = reader.GetSafeString(i++);
            course.Teacher = reader.GetSafeString(i++);
            course.Student = reader.DeserializeObject<IList<Student>>(i++); //Here an IList, which is an interface implemented by List, is used to 
            return course;                                                  //deserialize my JSON object of type Student
        }

    }
}
