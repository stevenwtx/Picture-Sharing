using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Entity
{
    public class User
    {
        public int Id { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public Gender Gender { get; set; }
        public byte Age { get; set; }
        public List<Picture> Pictures { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public User()
        {
            Pictures = new List<Picture>();
        }


        
    }

}
