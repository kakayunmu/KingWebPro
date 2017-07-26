using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using King.Domain.IRepositories;
using King.EntityFrameworkCore.Repositories;
using King.Application.UserApp;
using King.Application.MenuApp;
using King.Application.DepartmentApp;
using King.Domain.Entities;
using King.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Mvc.Formatters.Json;
using King.Application.RoleApp;
using King.Domain.IRepositories.WagesIRepositories;
using King.Application.StaffApp;
using King.Application.FixedProductApp;
using King.Application.WagesTemplateApp;

namespace King.MVC
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<KingDBContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);
            //使用缓存
            services.AddMemoryCache();
            //添加MVC支持
            services.AddMvc();
            //增加session服务
            services.AddSession();
            //添加用户管理服务
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuAppService, MenuAppService>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDepartmentAppService, DepartmentAppService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleAppService, RoleAppService>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IStaffAppService, StaffAppService>();
            services.AddScoped<IFixedProductRepository, FixedProductRepository>();
            services.AddScoped<IFixedProductAppService, FixedProductAppService>();
            services.AddScoped<IWagesTemplateRepository, WagesTemplateRepository>();
            services.AddScoped<IWagesTemplateAppService, WagesTemplateAppService>();
           

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole()
                .AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");             
            }
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "UserAuth",
                LoginPath = new Microsoft.AspNetCore.Http.PathString("/Login/Index/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory())
            });
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Login}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "API",                    
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
            //初始化映射关系
            King.Application.KingMapper.Initialize();
            //初始化数据
            SeedData.Initialize(app.ApplicationServices);
        }
    }
}
