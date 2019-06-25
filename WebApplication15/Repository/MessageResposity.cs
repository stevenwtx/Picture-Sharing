using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplication15.Model;
using WebApplication15.Models;

namespace WebApplication15.Repository
{
    public class MessageResposity : IMessageReposity
    {
        private readonly DataContext _context;
        public MessageResposity(DataContext context)
        {
            this._context = context;
        }

        public bool ClearMessage(int userId)
        {
            try
            {
                
                _context.Database.ExecuteSqlCommand($"update Liked set visible=0 where owerId=@{nameof(userId)}", new SqlParameter(nameof(userId), userId));
                return true;
            }
            catch (Exception)
            {
                
                throw;
                return false;
            }
        }

        public IEnumerable<MessageModel> GetLikes(int userId)
        {
           // String sql1 = "select UserName from(select * from Liked where {nameof(owerId)}=@{nameof(owerId)}) aa left join [User] on [User].Id=aa.UserId left join Pictures on Pictures.Id=aa.PictureId";
            var users = _context.User.Select(l => l.UserName).FromSql($"select UserName from(select * from Liked where owerId=@{nameof(userId)} and visible=1) aa left join [User] on [User].Id=aa.UserId left join Pictures on Pictures.Id=aa.PictureId", new SqlParameter(nameof(userId), userId)).ToList();
            //String sql2 = "select PicName from(select * from Liked where owerId={userId}) aa left join [User] on [User].Id=aa.UserId left join Pictures on Pictures.Id=aa.PictureId";
            var pics = _context.Pictures.Select(l => l.PicName).FromSql($"select PicName from(select * from Liked where owerId=@{nameof(userId)} and visible=1) aa left join [User] on [User].Id=aa.UserId left join Pictures on Pictures.Id=aa.PictureId", new SqlParameter(nameof(userId), userId)).ToList();

            return users.Zip(pics, (u, p) => new MessageModel { userName = u, picName = p });
        }
    }
}
