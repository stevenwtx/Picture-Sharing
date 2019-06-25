using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication15.Entity;

namespace WebApplication15.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public Gender Gender { get; set; }
        public byte Age { get; set; }
    }
}
