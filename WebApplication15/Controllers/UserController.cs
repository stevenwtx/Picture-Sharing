using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplication15.Entity;
using WebApplication15.Helpers;
using WebApplication15.Model;
using WebApplication15.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication15.Controllers
{
    [Route("api/User")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _iUserService;
        private readonly AppSettings _appSettings;

        public UserController(IUserService iUserService, IOptions<AppSettings> appSettings)
        {
            _iUserService = iUserService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginModel Loguser)
        {
            var user = _iUserService.Authenticate(Loguser.UserName, Loguser.Password);

            if (user == null)
                return BadRequest(new { message = "用户名或密码错误！" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            ///Startup.UploadUser.Add(user.UserName);
            JwtSecurityTokenHandler jst = new JwtSecurityTokenHandler();
            JwtSecurityToken username=jst.ReadJwtToken(tokenString);
            var aa=username.Claims.ToList()[0].Value;//用户名从token获取
            //var bb = aa[0];
            return Ok(new
            {
                user.Id,
                user.UserName,
                Token = tokenString,
                aa,
               // bb
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public JObject Register([FromBody]UserModel userModel)
        {
            // map dto to entity
            var user = new User
            {
                UserName = userModel.UserName,
                Password=userModel.Password,
                Gender=userModel.Gender,
                Age=userModel.Age
            };
            try
            {
                // save 
                _iUserService.Create(user, userModel.Password);
                {
                    JObject msg = new JObject();
                    msg.Add("status",1);
                    msg.Add("username", user.UserName);
                    return msg;
                }
                
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                JObject msg = new JObject();
                msg.Add("status", 0);
                msg.Add("error", "用户已存在");
                return msg;
            }
        }

        
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _iUserService.GetAll();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{token}")]
        public IActionResult GetById()
        {
            var BearToken = Request.Headers["Authorization"][0];
            string tokenString = BearToken.Split(" ")[1];
            JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();
            JwtSecurityToken jst = jsth.ReadJwtToken(tokenString);
            var username = jst.Claims.ToList()[0].Value;
            var user = _iUserService.GetByName(username);
            var likes = _iUserService.getLikeNum(user.Id);
            var uploadedable = "yes";
            if (Startup.UploadUser.Contains(username))
            {
                 uploadedable = "no";
            }
            return Ok(new {
                user.Id,
                user.UserName,
                user.Age,
                user.Gender,
                likes,
                uploadedable
                //test=Startup.UploadUser.ToArray()
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(string userName, [FromBody]UserModel userModel)
        {
            // map dto to entity and set id
            var user = new User
            {
                UserName = userModel.UserName,
                Password = userModel.Password,
                Gender = userModel.Gender,
                Age = userModel.Age
            };
            user.UserName = userName;

            try
            {
                // save 
                _iUserService.Update(user, userModel.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _iUserService.Delete(id);
            return Ok();
        }
    }
}
