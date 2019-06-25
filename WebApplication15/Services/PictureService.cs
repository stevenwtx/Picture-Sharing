using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication15.Entity;
using WebApplication15.Repository;

namespace WebApplication15.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _iPictureRepository;

        public PictureService(IPictureRepository pictureRepository)
        {
            _iPictureRepository = pictureRepository;
        }
        public Picture Create(Picture picture)
        {
            _iPictureRepository.Add(picture);
            return picture;
        }

        public void Delete(Picture picture)
        {
            _iPictureRepository.Remove(picture);
        }

        public IEnumerable<Picture> GetAll()
        {
            return _iPictureRepository.GetAll();
        }

        public IEnumerable<Picture> GetByCata(int cid)
        {
            return _iPictureRepository.GetPicturesByCatalogue(cid);
        }

        public IEnumerable<Picture> GetByUserName(string username,int visible)
        {
            return _iPictureRepository.GetByUser(username,visible);
        }

        public IEnumerable<Picture> GetSearch(string input)
        {
            return _iPictureRepository.searchPicture(input);
        }

        public IEnumerable<Picture> GetTop()
        {
            return _iPictureRepository.GetTop();
        }

        public Liked Like(int pictureId, int userId)
        {
            Picture p = _iPictureRepository.GetById(pictureId);
            Liked l = new Liked
            {
                owerId = p.UserId,
                UserId = userId,
                PictureId = pictureId,
                Visible=1
            };
            _iPictureRepository.addLike(l);
            return l;
        }

        public IEnumerable<Picture> Search(string name)
        {
            return _iPictureRepository.GetPictureByName(name);
        }

        public void Update(Picture picture)
        {
            _iPictureRepository.Update(picture);
        }

        
    }
}
