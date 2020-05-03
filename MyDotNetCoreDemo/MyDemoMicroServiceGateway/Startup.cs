using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace MyDemoMicroServiceGateway
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

            #region ids4  //IdentityServer4.AccessTokenValidati  
            //var authenticactionProvideKey = "UserGatewayKey";//策略 在配置文件中 配置作用于等等 
            //services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication(authenticactionProvideKey, options =>
            //    {
            //        options.Authority = "http://localhost:7000";//授权的服务器地址
            //        options.ApiName = "api";//
            //        options.RequireHttpsMetadata = false;
            //        options.SupportedTokens = SupportedTokens.Both;

            //    });
            #endregion

            services.AddOcelot().AddConsul().AddPolly();
            services.AddMvc();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "ApiGateway",
                    new OpenApiInfo
                    {
                        Title = "ApiGateway",
                        Version = "V1",
                        
                        //License = new OpenApiLicense
                        //{

                        //    Name = "AAA",
                        //    Url = new Uri("https://www.cnblogs.com/fallTakeMan/p/11289713.html")
                        //},
                        //Contact = new OpenApiContact
                        //{
                        //    Name ="AAA",
                        //    Email = "",
                        //    Url = new Uri("https://www.cnblogs.com/fallTakeMan/p/11289713.html")
                        //}
                    }


                    );

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var apis = new List<string> { "UserService" , "PayService" };//, "PayService"
            //app.UseMvc();
            app.UseSwagger()
               .UseSwaggerUI(options =>
               {
                   apis.ForEach(m =>
                   {
                      
                       options.SwaggerEndpoint($"/{m}/swagger.json", m);
                   });
               });
            app.UseOcelot();
        }
    }
}
