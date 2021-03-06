using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using MyDemoUserMicroServiceWebApi.Utility;

namespace MyDemoUserMicroServiceWebApi
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
            #region swagger 
            //注册 需要引用 包   Swashbuckle.AspNetCore
            //services.AddSwaggerGen(s =>
            //{
            //    #region 注册 Swagger
            //    s.SwaggerDoc("V1", new OpenApiInfo()
            //    {
            //        Title = "test",
            //        Version = "version-01",
            //        Description = "swagger 测试"
            //    });
            //    #endregion 
            //});

            if (Configuration["Swagger:IsRegist"] == bool.TrueString)
            {
                services.AddSwaggerGen(s =>
                {
                    s.SwaggerDoc(Configuration["Swagger:Name"], new OpenApiInfo()
                    {
                        Title = Configuration["Swagger:Title"],
                        Version = Configuration["Swagger:Version"],
                        Description = Configuration["Swagger:Description"]
                    });
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                    var xml = Path.Combine(basePath, "MyDemoUserMicroServiceWebApi.xml");//, entryAssemblyName + ".xml"
                    //嵌入 ef的xml
                    if (File.Exists(xml))
                    {
                        s.IncludeXmlComments(xml);
                    }

                });

            }

            #endregion



            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region 使用Swagger中间件
            if (Configuration["Swagger:IsRegist"] == bool.TrueString)
            {
             

                app.UseSwagger(s =>
                {
                   // s.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc = httpReq.Host.Value);
                    s.RouteTemplate = "{documentName}/swagger.json";

                });
        
                app.UseSwaggerUI(s =>
                {
                    s.SwaggerEndpoint($"/{Configuration["Swagger:Name"] }/swagger.json", Configuration["Swagger:Name"]);//允许加载 json文件  相对路径 微服务中才可以访问

                });
                app.UseCors(options =>
                {
                    options.WithOrigins("http://localhost:8888")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            }

         
            #endregion


            //实例启动时执行，且只执行一次
            this.Configuration.ConsulRegist();
        }
    }
}
