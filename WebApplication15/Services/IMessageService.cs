using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication15.Model;

namespace WebApplication15.Services
{
   public  interface IMessageService
    {
        IEnumerable<MessageModel> GetLikes(String userName);
        bool ClearMessage(String userName);
    }
}
