using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplication15.Entity;
using WebApplication15.Helpers;
using WebApplication15.Model;
using WebApplication15.Services;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace WebApplication15.Controllers
{
    

    [Route("api/Pic")]
    [ApiController]
    [Authorize]
    public class PictureController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private IPictureService _iPictureService;
        private IUserService _userService;
        private readonly AppSettings _appSettings;
        public PictureController(IPictureService pictureService,IHostingEnvironment hostingEnvironment,IUserService userService, IOptions<AppSettings> appSettings)
        {
            _iPictureService = pictureService;
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var Pics=_iPictureService.GetAll();
            return Ok(Pics);
        }

        [HttpGet("NumLeft")]
        [AllowAnonymous]
        public IActionResult GetNum()
        {
            return Ok(Startup.UpLoadNum);
        }

        [HttpGet("byUser")]
        public IActionResult GetByUser(int visible)
        {
            var BearToken = Request.Headers["Authorization"][0];
            string tokenString = BearToken.Split(" ")[1];
            JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();
            JwtSecurityToken jst = jsth.ReadJwtToken(tokenString);
            var userName = jst.Claims.ToList()[0].Value;
            var pics = _iPictureService.GetByUserName(userName, visible).ToList();
            JArray picArray = new JArray();
            foreach (var p in pics)
            {

                JObject pic = new JObject { { "name", p.PicName }, { "path", p.PicFileName } };
                picArray.Add(pic);

            }
            return Ok(picArray);
            
        }

        [HttpGet("byCata")]
        [AllowAnonymous]
        public IActionResult GetByCata(int cid)
        {
            if (cid == -1)
            {
                return Ok(_iPictureService.GetAll());
            }
            var id = -1;
            switch (cid)
            {
                case 0:
                    id = 1;
                    break;
                case 1:
                    id = 3;break;
                case 2:
                    id = 0;break;
                case 3:
                    id = 4;break;
                default:
                    id = 5;
                    break;
            }
            var pics = _iPictureService.GetByCata(id);
            return Ok(pics);
        }

        [HttpGet("like")]
        public IActionResult GetLike()
        {
            var BearToken = Request.Headers["Authorization"][0];
            string tokenString = BearToken.Split(" ")[1];
            JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();
            JwtSecurityToken jst = jsth.ReadJwtToken(tokenString);
            var userName = jst.Claims.ToList()[0].Value;
            if (Startup.LikeDic.ContainsKey(userName))
                return Ok(Startup.LikeDic[userName]);
            else return BadRequest();
        }

        [HttpGet("top")]
        [AllowAnonymous]
        public IActionResult GetTop()
        {
            var pics = _iPictureService.GetTop().ToList();
            JArray picArray = new JArray();
            foreach(var p in pics)
            {

                JObject pic = new JObject { {"name",p.PicName},{"path", p.PicFileName} };
                picArray.Add(pic);

            }
            return Ok(picArray);
        }

        [HttpGet("Search")]
        [AllowAnonymous]
        public IActionResult searchPic(string input)
        {
            return Ok(_iPictureService.GetSearch(input));
        }

        [HttpGet("cata")]
        [AllowAnonymous]
        public IActionResult GetCatalogue()
        {

            return Ok(_appSettings.Catalogue);
        }

        [HttpPost("upLoadPic")]
        public async Task<IActionResult> UploadIForm(IFormCollection formCollection)
        {
            FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
            var fileName = "";
            foreach (IFormFile foto in fileCollection)
            {
                long size = foto.Length;
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + foto.FileName;
                //edgeHTML内核
                if (fileName.Contains('\\'))
                {
                    var fileNames = fileName.Split('\\');
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileNames[fileNames.Length - 1];
                }
                var fileFolder = "";
                var filePath = "";
                //系统路径斜线区别
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    fileFolder = _hostingEnvironment.WebRootPath + "\\upload";
                    filePath = fileFolder + "\\" + fileName;
                }
                else
                {
                     fileFolder = _hostingEnvironment.WebRootPath + "/upload";
                     filePath = fileFolder + "/" + fileName;
                }
                
               
                
                
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }
                
            }
            return Ok(new
            {
                fileName
            });
        }

       [HttpPost("addPic")]
        public IActionResult AddPicture([FromBody] PictureModel pictureModel)
        {
            var BearToken = Request.Headers["Authorization"][0];
            string tokenString = BearToken.Split(" ")[1];
            JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();
            JwtSecurityToken jst = jsth.ReadJwtToken(tokenString);
            var owerName = jst.Claims.ToList()[0].Value;
            User u = _userService.GetByName(owerName);
            Picture picture = new Picture
            {
                PicName = pictureModel.picname,
                PicFileName=pictureModel.fileName,
                Catalogue=(Catalogue)pictureModel.catalogue,
                Visible=pictureModel.display,
                UserId=u.Id,
                UpLoadTime = DateTime.Now.ToString("yyyyMMdd")
            };
            _iPictureService.Create(picture);
            Startup.UploadUser.Add(u.UserName);
            Startup.UpLoadNum--;
            return Ok(new
            {
                picture.PicFileName,
                picture.PicName
            });
        }

        [HttpPost("likeThis")]
        public IActionResult LikeThis([FromBody]LikeModel likeModel)
        {
            //var header = Request.Headers.FirstOrDefault(x=>x.Value.Equals("Authorization")).Value[0];
            var tokenString = likeModel.TokenString;
            JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();
            JwtSecurityToken jst = jsth.ReadJwtToken(tokenString);
            var userName = jst.Claims.ToList()[0].Value;
            if (!Startup.LikeDic.ContainsKey(userName))
            {
                Liked l = _iPictureService.Like(likeModel.pictureId, _userService.GetByName(userName).Id);
                Startup.LikeDic.Add(userName, likeModel.pictureId);
                return Ok(new
                {
                    likeModel.pictureId,
                    userName
                });
            }
            return BadRequest("你今天已经点过赞了！");
        }

        /*public async Task<IActionResult> UploadPhotosAsync()
        {
            var fotos = Request.Form.Files;
            var filePath = "";
            foreach (var foto in fotos)
            {
                long size = foto.Length;
                var fileFolder = _hostingEnvironment.WebRootPath + "/image";
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + foto.FileName;
                filePath = fileFolder + "/" + fileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }
            }
            return Ok(new
            {
                filePath
            });
        }*/
    }

    }
