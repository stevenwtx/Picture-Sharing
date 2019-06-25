using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Models;
using Microsoft.AspNetCore.Rewrite;
using WebApplication15.Repository;
using WebApplication15.Services;
using System.Text;
using WebApplication15.Helpers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;

namespace WebApplication15
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static List<string> UploadUser=new List<string>();
        public static Dictionary<string,int> LikeDic=new Dictionary<string,int>();
        public static int UpLoadNum;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddTimedJob();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPictureRepository, PictureRepository>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IMessageReposity, MessageResposity>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddDbContext<DataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            var userId = context.Principal.Identity.Name;
                            var user = userService.GetByName(userId);
                            if (user == null)
                            {
                                // return unauthorized if user no longer exists
                                context.Fail("Unauthorized");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            UpLoadNum = appSettings.AlowedPicUpLoadNum;

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var rewrite = new RewriteOptions()
                .AddRewrite("home", "index.html", skipRemainingRules: true)
                .AddRewrite("mobile", "/mobile/index.html", skipRemainingRules: true)
                .AddRewrite("search", "/mobile/search.html", skipRemainingRules: true)
                .AddRewrite("activity", "/mobile/activity.html", skipRemainingRules: true)
                .AddRewrite("user","account.html",skipRemainingRules:true)
                .AddRewrite("picAddInfo","/mobile/picinfo.html",skipRemainingRules:true)
                .AddRewrite("account", "/mobile/account.html", skipRemainingRules: true);
            app.UseRewriter(rewrite);
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Add("/home");
             app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
            app.UseTimedJob();
            app.UseAuthentication();
            app.UseSession();
            app.UseMvc();
            
        }
    }
}
