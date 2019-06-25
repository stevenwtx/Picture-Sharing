using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication15.Entity;
using WebApplication15.Model;
using WebApplication15.Models;

namespace WebApplication15.Repository
{

    
    public class PictureRepository : IPictureRepository
    {
        private readonly DataContext _context;

        public PictureRepository(DataContext context)
        {
            _context = context;
        }
        public int Add(Picture p)
        {
            _context.Pictures.Add(p);
            return _context.SaveChanges();
        }

        public bool Any(Expression<Func<Picture, bool>> predicate)
        {
            return _context.Pictures.Any(predicate);
        }

        public IEnumerable<Picture> GetAll()
        {
            return _context.Pictures.Where(x=>x.Visible==1);
        }

        public IEnumerable<Picture> GetPictureByName(string PicName)
        {

            return _context.Pictures.Where(x => EF.Functions.Like(x.PicName, "%" + PicName + "%"));
        }

        public IEnumerable<Picture> GetPicturesByCatalogue(int num) {

            return _context.Pictures.Where(x=>x.Visible==1).Where(x => x.Catalogue == (Catalogue)num);
        }

        public IEnumerable<Picture> GetByUser(String UserName,int visible)
        {
           User u= _context.User.SingleOrDefault(x => x.UserName == UserName);
            return _context.Pictures.Where(x => x.UserId == u.Id).Where(x=>x.Visible==visible);
        }

        public int Remove(Picture picture)
        {
            _context.Pictures.Remove(picture);
            return _context.SaveChanges();
        }

        public int Update(Picture picture)
        {
            _context.Pictures.Update(picture);
            return _context.SaveChanges();
        }

        public IEnumerable<Picture> GetTop()
        {
            /*String sql = "select top(3) Id,UserId,owerId,Pictureid " +
                 "from (select Id 'Id',UserId 'UserId',owerId'owerId',Pictureid 'Pictureid',count(id) 'nums' " +
                 "from Liked " +
                 "group by Pictureid,Id,UserId,owerId)pic " +
                 "order by nums";*/
            String sql = "select Top(3)PictureId from Liked group by PictureId order by count(Id) DESC";
            var likes= _context.Liked.Select(l=>new { l.PictureId}).FromSql(sql).ToList();
            
            List<int> pids = new List<int>();
            foreach(var l in likes)
            {
                pids.Add(l.PictureId);
            }
            
            return _context.Pictures.Where(x => pids.Contains(x.Id));
            
        }

        public Picture GetById(int pictureId)
        {
            return _context.Pictures.Find(pictureId);
        }

        public int addLike(Liked like)
        {
            _context.Liked.Add(like);
            return _context.SaveChanges();
        }

       

        

        public IEnumerable<Picture> searchPicture(string input)
        {
            return _context.Pictures.Where(x => x.Visible == 1 && x.PicName.Contains(input));
        }
    }
}
