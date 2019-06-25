using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication15.Entity
{
    public class Liked
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int owerId { get; set; }
        public int PictureId { get; set; }
        public int Visible { get; set; }
        public Picture Picture { get; set; }
    }
}
