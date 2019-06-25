using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication15.Entity;
using WebApplication15.Model;
using WebApplication15.Repository;

namespace WebApplication15.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _iUserRepository;

        public UserService(IUserRepository iUserRepository)
        {
            _iUserRepository = iUserRepository;
        }

        public User Authenticate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return null;

            var user = _iUserRepository.GetUserByUserName(userName);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public User Create(User user, string password)
        {

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception();


            if (_iUserRepository.Any(x => x.UserName == user.UserName))
                throw new Exception();

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _iUserRepository.Add(user);

            return user;
        }

        public void Delete(string username)
        {
            var user = _iUserRepository.GetUserByUserName(username);
            if (user != null)
            {
                _iUserRepository.Remove(user);
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _iUserRepository.GetAll();
        }

        public User GetById(int id)
        {
            return _iUserRepository.GetById(id);
        }

        public User GetByName(string username)
        {
            return _iUserRepository.GetUserByUserName(username);
        }

        public void Update(User user, string password = null)
        {
            throw new NotImplementedException();
        }


        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public int getLikeNum(int id)
        {
            var likes = _iUserRepository.GetLikeds(id);
            return likes.Count();
        }
    }
}
