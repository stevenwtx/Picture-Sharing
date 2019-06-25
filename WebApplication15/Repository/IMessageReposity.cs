using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication15.Model;

namespace WebApplication15.Repository
{
    public interface IMessageReposity
    {
        IEnumerable<MessageModel> GetLikes(int userId);
        bool ClearMessage(int userId);
    }
}
