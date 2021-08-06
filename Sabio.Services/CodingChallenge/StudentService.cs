using Sabio.Data.Providers;
using Sabio.Services.Interfaces.CodingChallenge;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services.codingChallenge
{
    public class StudentService : IStudentService
    {
        IDataProvider _data = null;

        public StudentService(IDataProvider data)
        {

            _data = data;

        }

        public void Delete(int id)
        {

            string procName = "[dbo].[Students_Delete]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);

                });
        }

    }
}
