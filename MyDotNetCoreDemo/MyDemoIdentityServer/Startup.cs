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
            //���� connect/token  post ������  client_id client_secret grant_gype=client_credentials
            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()//Ĭ�ϰ䷢֤��
            //    .AddInMemoryClients(Config.Clients)
            //    .AddInMemoryApiResources(Config.GetApiResources());

            #region  ���ݿⷽʽ

            services.AddIdentityServer()
               .AddDeveloperSigningCredential()//Ĭ�ϰ䷢֤��
               .AddInMemoryApiResources(Config.GetApiResources())//������Щ�ӿ�
               //.AddInMemoryClients(OAuthMemoryData.GetClients())
               .AddClientStore<ClientStore>()//�����Щ�ͻ�������
               //.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
               .AddResourceOwnerValidator<RoleTestResourceOwnerPasswordValidator>()
               .AddExtensionGrantValidator<WeiXinOpenGrantValidator>()
               .AddProfileService<UserProfileService>();//���΢�Ŷ��Զ��巽ʽ����֤

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
