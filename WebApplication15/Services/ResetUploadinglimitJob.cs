using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pomelo.AspNetCore.TimedJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication15.Helpers;

namespace WebApplication15.Services
{
    public class ResetUploadinglimitJob:Job
    {
        private readonly AppSettings _appSettings;
        public ResetUploadinglimitJob(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        [Invoke(Begin ="2019-5-29 00:00",Interval =24*3600*1000,SkipWhileExecuting =true)]
        public void Run(IConfiguration configuration)
        {

            Startup.UpLoadNum = _appSettings.AlowedPicUpLoadNum;
            Startup.UploadUser.Clear();
            Startup.LikeDic.Clear();
        }
    }
}
