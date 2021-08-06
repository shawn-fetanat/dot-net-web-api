using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface IUsersService
    {
        int Add(UserAddRequest model, int userId);
        void Delete(int id);
        User Get(int id);
        List<User> GetTop();
        void Update(UserUpdateRequest model);
        List<User> Search(string q);
        Paged<User> GetPage(int pageIndex, int pageSize);
    }
}