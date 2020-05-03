using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace MyDemoIdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
                new ApiResource("api1", "My API")

            };//那些api}
        }

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                //new Client
                //{
                //    ClientId="client",//客户端唯一的标识 可以理解为账号
                //    AllowedGrantTypes =GrantTypes.ClientCredentials,//授权方式 客户端认证 只需要 ClientId+ClientSecret
                //    ClientSecrets={
                //    new Secret("aju".Sha256())
                //    },//客户端密码 进行加密 可以理解为密码
                //    AllowedScopes={ "api1"}//允许访问的资源


                //}

                 new Client()
                {
                    ClientId = "ClientId",
                    AllowedGrantTypes = new List<string>()
                    {
                        IdentityServer4.Models.GrantTypes.ResourceOwnerPassword.FirstOrDefault(),//Resource Owner Password模式
                       "weixinopen",
                    },
                    ClientSecrets = {new Secret("mima".Sha256()) },
                    AllowOfflineAccess = true,//如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                    AllowedScopes= {
                       "api_mingcheng",
                        StandardScopes.OfflineAccess,
                    },
                    AccessTokenLifetime = 3000,//过期时间
                },
            };
    }
}
