using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Entity
{
    public class Picture
    {
        public int Id { get; set; }
        public String PicName { get; set; }
        public String PicFileName { get; set; }
        public Catalogue Catalogue { get; set; }
        public String UpLoadTime { get; set; }
        public int Visible { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public List<Liked> Likeds { get; set; }
        public Picture()
        {
            Likeds = new List<Liked>();
        }
    }

    
}
