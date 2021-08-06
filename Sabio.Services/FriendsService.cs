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
    public class FriendsService : IFriendsService
    {
        IDataProvider _data = null;
        public FriendsService(IDataProvider data)
        {

            _data = data;

        }

        public void Delete(int id)
        {

            string procName = "[dbo].[FriendsFirst_Delete]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);

                });
        }
        public void Update(FriendUpdateRequest model)
        {

            string procName = "[dbo].[FriendsFirst_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Title", model.Title);
                col.AddWithValue("@Bio", model.Bio);
                col.AddWithValue("@Summary", model.Summary);
                col.AddWithValue("@Headline", model.Headline);
                col.AddWithValue("@Slug", model.Slug);
                col.AddWithValue("@Skills", model.Skills);
                col.AddWithValue("@StatusId", model.StatusId);
                col.AddWithValue("@PrimaryImage", model.PrimaryImage);
                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);

        }      

        public int Add(FriendAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[FriendsFirst_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", model.UserId);
                col.AddWithValue("@Title", model.Title);
                col.AddWithValue("@Bio", model.Bio);
                col.AddWithValue("@Summary", model.Summary);
                col.AddWithValue("@Headline", model.Headline);
                col.AddWithValue("@Slug", model.Slug);
                col.AddWithValue("@NewSkills", model.Skills);
                col.AddWithValue("@PrimaryImage", model.PrimaryImage);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {

                object oId = returnCollection["@Id"].Value;
                Int32.TryParse(oId.ToString(), out id);
                Console.WriteLine("");

            });
            return id;
        }

        public Friend Get(int id)
        {

            Friend friend = new Friend();

            if (id > 1000)
            {
                throw new ArgumentOutOfRangeException("This is a simulated error.");
            }

            string procName = "[dbo].[FriendsFirst_SelectById]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {

                parameterCollection.AddWithValue("@Id", id);


            }, delegate (IDataReader reader, short set)
            {
                friend = MapFriend(reader);
            }
            );
            return friend;
        }

        public List<Friend> GetTop()
        {
            List<Friend> list = null;
            string procName = "[dbo].[FriendsFirst_SelectRandom50]";
            _data.ExecuteCmd(procName, inputParamMapper: null
                , singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Friend friend = new Friend();

                    int i = 0;

                    friend.Id = reader.GetSafeInt32(i++);
                    friend.UserId = reader.GetSafeString(i++);
                    friend.DateAdded = reader.GetSafeDateTime(i++);
                    friend.DateModified = reader.GetSafeDateTime(i++);
                    friend.Title = reader.GetSafeString(i++);
                    friend.Bio = reader.GetSafeString(i++);
                    friend.Summary = reader.GetSafeString(i++);
                    friend.Headline = reader.GetSafeString(i++);
                    friend.Slug = reader.GetSafeString(i++);
                    friend.StatusId = reader.GetSafeString(i++);
                    friend.Skills = reader.GetSafeString(i++);
                    friend.PrimaryImage = reader.GetSafeString(i++);



                    if (list == null)
                    {
                        list = new List<Friend>();
                    }
                    list.Add(friend);
                }
            );
            return list;
        }

        public Paged<Friend> Search(int pageIndex, int pageSize, string q)
        {
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            int totalCount = 0;

            string procName = "[dbo].[FriendsFirst_Search]";

            _data.ExecuteCmd(procName, (param) =>
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@Query", q);

            }, (reader, recordSetIndex) =>
            {

                Friend aFriend = MapFriend(reader);
                totalCount = reader.GetSafeInt32(12);


                if (list == null)
                {
                    list = new List<Friend>();
                }

                list.Add(aFriend);
            }
            );
            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }      

        public Paged<Friend> GetPage(int pageIndex, int pageSize)
        {
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            int totalCount = 0;

            string procName = "[dbo].[FriendsFirst_SelectPaginated]";

            _data.ExecuteCmd(procName, (param) =>
            {

                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);

            }, (reader, recordSetIndex) =>
            {

                Friend aFriend = MapFriend(reader);
                totalCount = reader.GetSafeInt32(12);


                if (list == null)
                {
                    list = new List<Friend>();
                }

                list.Add(aFriend);
            }
            );
            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        private static Friend MapFriend(IDataReader reader)
        {
            Friend friend = new Friend();

            int i = 0;

            friend.Id = reader.GetSafeInt32(i++);
            friend.UserId = reader.GetSafeString(i++);
            friend.DateAdded = reader.GetSafeDateTime(i++);
            friend.DateModified = reader.GetSafeDateTime(i++);
            friend.Title = reader.GetSafeString(i++);
            friend.Bio = reader.GetSafeString(i++);
            friend.Summary = reader.GetSafeString(i++);
            friend.Headline = reader.GetSafeString(i++);
            friend.Slug = reader.GetSafeString(i++);
            friend.Skills = reader.GetSafeString(i++);
            friend.StatusId = reader.GetSafeString(i++);
            friend.PrimaryImage = reader.GetSafeString(i++);
            return friend;
        }
    }
}
