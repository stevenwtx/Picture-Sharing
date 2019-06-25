using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication15.Model;
using WebApplication15.Repository;

namespace WebApplication15.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageReposity _imessageReposity;
        private readonly IUserRepository _iUserRepository;
        public MessageService(IMessageReposity messageReposity, IUserRepository userRepository)
        {
            _imessageReposity = messageReposity;
            _iUserRepository = userRepository;
        }

        public bool ClearMessage(string userName)
        {
            var user = _iUserRepository.GetUserByUserName(userName);
            return _imessageReposity.ClearMessage(user.Id);
        }

        public IEnumerable<MessageModel> GetLikes(String userName)
        {
            var user = _iUserRepository.GetUserByUserName(userName);
            return _imessageReposity.GetLikes(userId: user.Id);
        }
    }
}
