using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IFriendsService
    {
        public void Delete(int id);
        int Add(FriendAddRequest model);
        Friend Get(int id);
        List<Friend> GetTop();
        Paged<Friend> Search(int pageIndex, int pageSize, string q);
        void Update(FriendUpdateRequest model);

        Paged<Friend> GetPage(int pageIndex, int pageSize);
    }
}