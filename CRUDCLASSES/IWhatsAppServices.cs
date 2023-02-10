using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CRUDCLASSES.Model;

namespace CRUDCLASSES
{
    public interface IWhatsAppServices : IDisposable
    {
        Task<long> CreateUser(UserViewModel user);

        Task<bool> UpdateUser(int userId, UserViewModel user);

        //void DeleteUser (int userId);
        Task<bool> DeleteUser(int UserId);

        Task<UserViewModel> GetUser(int id);

        Task<IEnumerable<UserViewModel>> GetUsers();
    }
}
