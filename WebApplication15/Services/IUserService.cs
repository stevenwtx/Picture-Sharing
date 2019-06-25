using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication15.Entity;

namespace WebApplication15.Services
{
    public interface IUserService
    {
        User Authenticate(string userName, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        User GetByName(String username);
        void Update(User user, string password = null);
        void Delete(string username);
        int getLikeNum(int id);
    }
}
