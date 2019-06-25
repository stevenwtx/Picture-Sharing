using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication15.Entity;

namespace WebApplication15.Services
{
    public interface IPictureService
    {
        IEnumerable<Picture> GetAll();
        Picture Create(Picture picture);
        IEnumerable<Picture> GetByUserName(String username, int visible);
        IEnumerable<Picture> Search(String name);
        void Update(Picture picture);
        void Delete(Picture picture);
        IEnumerable<Picture> GetTop();
        Liked Like(int pictureId,int userId);
        IEnumerable<Picture> GetByCata(int cid);
        IEnumerable<Picture> GetSearch(string input);
    }
}
