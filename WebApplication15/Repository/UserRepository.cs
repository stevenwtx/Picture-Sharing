using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication15.Entity;
using WebApplication15.Models;

namespace WebApplication15.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public int Add(User user)
        {
            _context.User.Add(user);
            return _context.SaveChanges();
        }

        public bool Any(Expression<Func<User, bool>> predicate)
        {
            return _context.User.Any(predicate);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.User;
        }

        public User GetById(int id)
        {
            return _context.User.Find(id);
        }

        public User GetUserByUserName(string userName)
        {
            return _context.User.SingleOrDefault(x => x.UserName == userName);
        }

        public int Update(User user)
        {
            _context.User.Update(user);
            return _context.SaveChanges();
        }

        public int Remove(User user)
        {
            _context.User.Remove(user);
            return _context.SaveChanges();
        }

        public IEnumerable<Liked> GetLikeds(int id)
        {
            return _context.Liked.Where(x => x.owerId == id);
        }
    }
}
