using System.Data.SqlClient;
using Sabio.Models.Domain;
using Sabio.Services;
using Sabio.Data;
using System;
using System.Collections.Generic;
using Sabio.Models.Requests;

namespace Sabio.Db.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region - Additional Exercise Comments 
            //Here are two example connection strings. Please check with the wiki and video courses to help you pick an option

            //string connString = @"Data Source=ServerName_Or_IpAddress;Initial Catalog=DB_Name;User ID=SabioUser;Password=Sabiopass1!"; 
            #endregion

            string connString = @"Data Source=104.42.194.102;Initial Catalog=C105_shawn.fetanat_gmail;User ID=C105_shawn.fetanat_gmail_User;Password=C105_shawn.fetanat_gmail_UserBB564BFB";

            TestConnection(connString);

            #region - Friend Service - OK
            SqlDataProvider provider = new SqlDataProvider(connString);

            FriendsService friendService = new FriendsService(provider);

            Friends aFriend = friendService.Get(9);

            Friends missingFriend = friendService.Get(9999);

            List<Friends> friends = friendService.GetTop();
            #endregion

            #region - Friend Requests - OK
            FriendAddRequest friendRequest = new FriendAddRequest();

            friendRequest.Title = "New Title";
            friendRequest.Bio = "New Bio";
            friendRequest.Summary = "New Summary";
            friendRequest.Headline = "New Headline";
            friendRequest.Slug = "New Slug";
            friendRequest.Skills = "New SKills";
            friendRequest.StatusId = "Active";
            friendRequest.PrimaryImage = "www.newprimaryimage.com";

            int newFriendId = friendService.Add(friendRequest);

            FriendUpdateRequest friendUpdateRequest = new FriendUpdateRequest();

            friendUpdateRequest.Title = "Updated Title";
            friendUpdateRequest.Bio = "Updated Bio";
            friendUpdateRequest.Summary = "Updated Summary";
            friendUpdateRequest.Headline = "Updated Headline";
            friendUpdateRequest.Slug = "Updated Slug";
            friendUpdateRequest.Skills = "Updated SKills";
            friendUpdateRequest.StatusId = "Flagged";
            friendUpdateRequest.PrimaryImage = "www.updatedprimaryimage.com";
            friendUpdateRequest.Id = newFriendId;

            friendService.Update(friendUpdateRequest);

            Friends goodFriend = friendService.Get(newFriendId);

            Console.WriteLine(goodFriend.Id.ToString());

            #endregion

            #region - Users Service - OK
            UsersService usersService = new UsersService(provider);

            User aUser = usersService.Get(9);

            User missingUser = usersService.Get(9999);

            List<User> users = usersService.GetTop();
            #endregion

            #region - Users Requests - OK
            UserAddRequest userRequest = new UserAddRequest();

            userRequest.FirstName = "Joe";
            userRequest.LastName = "Biden";
            userRequest.Email = "joe.biden@gmail.com";
            userRequest.Password = "SabioPassword1!";
            userRequest.PasswordConfirm = "SabioPassword1!";
            userRequest.AvatarUrl = "www.imgur.com/123";
            userRequest.TenantId = "1234ABCDE";

            int newUserId = usersService.Add(userRequest);

            UserUpdateRequest userUpdateRequest = new UserUpdateRequest();

            userUpdateRequest.FirstName = "Joe";
            userUpdateRequest.LastName = "Biden";
            userUpdateRequest.Email = "joe.biden@gmail.com";
            userUpdateRequest.Password = "SabioPassword1!";
            userUpdateRequest.PasswordConfirm = "SabioPassword1!";
            userUpdateRequest.AvatarUrl = "www.imgur.com/123";
            userUpdateRequest.TenantId = "1234ABCDE";
            userUpdateRequest.Id = newUserId;

            usersService.Update(userUpdateRequest);

            User goodUser = usersService.Get(newUserId);

            Console.WriteLine("You just updated Id#: ");
            Console.WriteLine(goodUser.Id.ToString());

            UserDeleteRequest userDeleteRequest = new UserDeleteRequest();

            userDeleteRequest.Id = newUserId;
            
            usersService.Delete(userDeleteRequest.Id);

            User deletedUser = usersService.Get(userDeleteRequest.Id);

            if (deletedUser == null)
            {
                Console.WriteLine("You just deleted Id#: ");
                Console.WriteLine(userDeleteRequest.Id.ToString());
            }
            else
            {
                Console.WriteLine("Delete was unsuccessful.");
            }

            #endregion

            #region - Address Services - OK            

            AddressService addressService = new AddressService(provider); //remember that here the constructor is being invoked 

            Addresses aAddress = addressService.Get(9);

            Addresses missingAddress = addressService.Get(9999999);

            List<Addresses> addresses = addressService.GetTop();
            #endregion

            #region - Address Requests - OK
            AddressAddRequest request = new AddressAddRequest();

            request.LineOne = "18 Primrose";
            request.SuiteNumber = 18;
            request.City = "Irvine";
            request.State = "CA";
            request.PostalCode = "92604";
            request.IsActive = true;
            request.Lat = 41.7691;
            request.Long = -72.701;

            int newId = addressService.Add(request);

            AddressUpdateRequest updateRequest = new AddressUpdateRequest();

            updateRequest.LineOne = "123 Sesame St";
            updateRequest.SuiteNumber = 123;
            updateRequest.City = "Fake City";
            updateRequest.State = "CA";
            updateRequest.PostalCode = "12345";
            updateRequest.IsActive = true;
            updateRequest.Lat = 40.7691;
            updateRequest.Long = -71.701;
            updateRequest.Id = newId;

            addressService.Update(updateRequest);

            Addresses goodAddress = addressService.Get(newId);

            Console.WriteLine(goodAddress.Id.ToString()); 
            #endregion

            Console.ReadLine();
        }

        private static void TestConnection(string connString)
        {
            bool isConnected = IsServerConnected(connString);
            Console.WriteLine("DB isConnected = {0}", isConnected);
        }

        private static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
    }
}
