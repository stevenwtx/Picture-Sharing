using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication15.Entity;

namespace WebApplication15.Repository
{
    public interface IPictureRepository
    {
        IEnumerable<Picture> GetPictureByName(String name);
        IEnumerable<Picture> GetAll();
        IEnumerable<Picture> GetByUser(String UserName, int visible);
        Picture GetById(int pictureId);
        IEnumerable<Picture> GetTop();
        bool Any(Expression<Func<Picture, bool>> predicate);
        int Add(Picture p);
        int Update(Picture picture);
        int Remove(Picture picture);
       // int Like(int pictureid,int userid);
        IEnumerable<Picture> GetPicturesByCatalogue(int num);
        IEnumerable<Picture> searchPicture(string input);
        int addLike(Liked like);
    }
}
