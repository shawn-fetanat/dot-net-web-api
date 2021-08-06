using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class UsersService : IUsersService
    {
        IDataProvider _data = null;
        public UsersService(IDataProvider data)
        {
            _data = data;
        }

        public void Delete(int id)
        {

            string procName = "[dbo].[Users_Delete]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);

                });
        }

        public int Add(UserAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Users_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@FirstName", model.FirstName);
                    col.AddWithValue("@LastName", model.LastName);
                    col.AddWithValue("@Email", model.Email);
                    col.AddWithValue("@Password", model.Password);
                    col.AddWithValue("@PasswordConfirm", model.PasswordConfirm);
                    col.AddWithValue("@AvatarUrl", model.AvatarUrl);
                    col.AddWithValue("@TenantId", model.TenantId);

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

        public void Update(UserUpdateRequest model)
        {

            string procName = "[dbo].[Users_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", model.Id);
                    col.AddWithValue("@FirstName", model.FirstName);
                    col.AddWithValue("@LastName", model.LastName);
                    col.AddWithValue("@Email", model.Email);
                    col.AddWithValue("@Password", model.Password);
                    col.AddWithValue("@PasswordConfirm", model.PasswordConfirm);
                    col.AddWithValue("@AvatarUrl", model.AvatarUrl);
                    col.AddWithValue("@TenantId", model.TenantId);

                }, returnParameters: null);
        }

        public User Get(int id)
        {
            string procName = "[dbo].[Users_SelectById]";

            User user = null;

            if (id > 1000)
            {
                throw new ArgumentOutOfRangeException("This is a simulated error.");
            }


            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {

                parameterCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {

                user = MapUser(reader);
            }
            );
            return user;
        }

        public List<User> GetTop()
        {
            List<User> list = null;

            string procName = "[dbo].[Users_SelectRandom50]";

            _data.ExecuteCmd(procName, inputParamMapper: null
                , singleRecordMapper: delegate (IDataReader reader, short set)
                 {
                     User aUser = MapUser(reader);

                     if (list == null)
                     {
                         list = new List<User>();
                     }
                    
                     list.Add(aUser);

                 });

            return list;
        }

        public List<User> Search(string q)
        {
            List<User> list = null;
            string procName = "[dbo].[Users_Search]";
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {

                parameterCollection.AddWithValue("@Query", q);

            }, delegate (IDataReader reader, short set)
            {

                User aUser = MapUser(reader);

                if (list == null)
                {
                    list = new List<User>();
                }

                list.Add(aUser);
            }
            );            
            return list;
        }

        public Paged<User> GetPage(int pageIndex, int pageSize)
        {
            Paged<User> pagedList = null;
            List<User> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Users_SelectPaginated]";

            _data.ExecuteCmd(procName, (param) =>
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);

            }, (reader, recordSetIndex) =>
            {

                User aUser = MapUser(reader);
                totalCount = reader.GetSafeInt32(10);

                if (list == null)
                {
                    list = new List<User>();
                }

                list.Add(aUser);
            }
            );
            if (list != null)
            {
                pagedList = new Paged<User>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        private static User MapUser(IDataReader reader)
        {
            User aUser = new User();

            int i = 0;

            aUser.Id = reader.GetSafeInt32(i++);
            aUser.DateAdded = reader.GetSafeUtcDateTime(i++);        
            aUser.DateModified = reader.GetSafeUtcDateTime(i++);
            aUser.FirstName = reader.GetSafeString(i++);
            aUser.LastName = reader.GetSafeString(i++);
            aUser.Email = reader.GetSafeString(i++);
            aUser.Password = reader.GetSafeString(i++);
            aUser.PasswordConfirm = reader.GetSafeString(i++);
            aUser.AvatarUrl = reader.GetSafeString(i++);       
            aUser.TenantId = reader.GetSafeString(i++);
            return aUser;
        }
    }
}
