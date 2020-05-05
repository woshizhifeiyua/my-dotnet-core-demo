using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyDemoPayMicroServiceWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddCommandLine(args)//支持命令行
            .Build();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration((hostContext, config) =>
                //{
                //    var env = hostContext.HostingEnvironment;
                //    config.SetBasePath(Path.Combine(env.ContentRootPath, "Configuration"))
                //        .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                //        .AddJsonFile(path: $"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                //})
            .ConfigureLogging(logging => //支持IOC  控制反转
            {
                logging.AddFilter("System", LogLevel.Information);  //忽略系统日志
                logging.AddFilter("Microsoft", LogLevel.Information);   //忽略系统日志
                logging.AddLog4Net("Config/log4net.Config");
            })
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
             });
    }
}
