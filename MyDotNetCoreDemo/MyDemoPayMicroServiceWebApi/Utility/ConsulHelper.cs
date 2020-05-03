using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDemoUserMicroServiceWebApi.Utility
{
    public static class ConsulHelper
    {
        /// <summary>
        /// consul的注册
        /// </summary>
        /// <param name="configuration"></param>
        public static void ConsulRegist(this IConfiguration configuration)
        {

            //https://www.cnblogs.com/guolianyu/p/9557225.html
            if (configuration["ConsulRegist:IsRegist"] == bool.TrueString)
            {
                ConsulClient client = new ConsulClient(c =>
                {
                    c.Address = new Uri(configuration["ConsulRegist:Address"]); //consul 的地址
                    c.Datacenter = "dc1";
                });
               
                
                string ip = configuration["ConsulRegist:ip"];
                int port = int.Parse(configuration["ConsulRegist:port"]);//命令行参数必须传入
                int weight = string.IsNullOrWhiteSpace(configuration["ConsulRegist:weight"]) ? 1 : int.Parse(configuration["ConsulRegist:weight"]);

 
                client.Agent.ServiceRegister(new AgentServiceRegistration()
                {
                    ID = configuration["ConsulRegist:Id"] + ip + "_" + port,//唯一的
                    Name = configuration["ConsulRegist:Name"],//组名称-Group
                    Address = ip,//该服务注册运行的ip  其实应该写ip地址
                    Port = port,//该服务的端口 不同实例 
                    Tags = new string[] { weight.ToString() },//标签
                    Check = new AgentServiceCheck()//配置心跳检查的
                    {
                        Interval = TimeSpan.FromSeconds(int.Parse(configuration["ConsulRegist:AgentServiceCheck:Interval"].ToString())),//心跳检查
                        HTTP = $"http://{ip}:{port}" + configuration["ConsulRegist:AgentServiceCheck:HTTP"],//检查的地址
                        Timeout = TimeSpan.FromSeconds(int.Parse(configuration["ConsulRegist:AgentServiceCheck:DeregisterCriticalServiceAfter"].ToString())),//超时时间
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(int.Parse(configuration["ConsulRegist:AgentServiceCheck:DeregisterCriticalServiceAfter"].ToString()))//出错后多久去掉
                    }
                });



                Console.WriteLine($"http://{ip}:{port}完成注册");

            }


            
        }
    }
}
