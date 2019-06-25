using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Entity;

namespace WebApplication15.Models
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>()
                .HasOne(x => x.User).WithMany(x => x.Pictures).HasForeignKey(x => x.UserId);

          

            modelBuilder.Entity<Liked>()
                .HasOne(x => x.Picture).WithMany(x => x.Likeds).HasForeignKey(x => x.PictureId);

          
        }

        public DbSet<User> User { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Liked> Liked { get; set; }
    }
}
