using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyDemoIdentityServer.Tool;

namespace MyDemoIdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            //访问 connect/token  post 、参数  client_id client_secret grant_gype=client_credentials
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()//默认颁发证书
            //    .AddInMemoryClients(Config.Clients)
            //    .AddInMemoryApiResources(Config.GetApiResources());

            #region  数据库方式

            services.AddIdentityServer()
               .AddDeveloperSigningCredential()//默认颁发证书
               .AddInMemoryApiResources(Config.GetApiResources())//访问那些接口
               //.AddInMemoryClients(OAuthMemoryData.GetClients())
               .AddClientStore<ClientStore>()//添加那些客户端连接
               //.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
               .AddResourceOwnerValidator<RoleTestResourceOwnerPasswordValidator>()
               .AddExtensionGrantValidator<WeiXinOpenGrantValidator>()
               .AddProfileService<UserProfileService>();//添加微信端自定义方式的验证

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
