using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication15.Entity;

namespace WebApplication15.Repository
{
    public interface IUserRepository
    {
        User GetUserByUserName(String UserName);
        IEnumerable<User> GetAll();
        User GetById(int id);
        bool Any(Expression<Func<User, bool>> predicate);
        int Add(User user);
        int Update(User user);
        int Remove(User user);
        IEnumerable<Liked> GetLikeds(int id);
    }
}
