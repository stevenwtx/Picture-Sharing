using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication15.Services;

namespace WebApplication15.Controllers
{
    [Route("api/Message")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var BearToken = Request.Headers["Authorization"][0];
            string tokenString = BearToken.Split(" ")[1];
            JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();
            JwtSecurityToken jst = jsth.ReadJwtToken(tokenString);
            var userName = jst.Claims.ToList()[0].Value;
            return Ok(_messageService.GetLikes(userName));
        }

        [HttpPut("clear")]
        public IActionResult Clear()
        {
            var BearToken = Request.Headers["Authorization"][0];
            string tokenString = BearToken.Split(" ")[1];
            JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();
            JwtSecurityToken jst = jsth.ReadJwtToken(tokenString);
            var userName = jst.Claims.ToList()[0].Value;
            if (_messageService.ClearMessage(userName))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}