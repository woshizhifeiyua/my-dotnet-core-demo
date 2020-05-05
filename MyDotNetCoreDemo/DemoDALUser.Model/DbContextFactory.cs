using DemoFarmWork.Enum;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDALUser.Model
{
    public class DbContextFactory : IDbContextFactory
    {

        private IConfiguration _configuration;

        private string[] ReadConn = null;

        public DbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            ReadConn = _configuration.GetConnectionString("ConnectionStrings:Rrite").Split(',');
        }

        private string ConnRead()
        {
            //方式很多 可以 轮询  权重 等等
            int index = new Random().Next(0, ReadConn.Length - 1);
            return ReadConn[index];


        }

        public UserDbContext CreateContext(ConnDbContextEnumType connDbContextEnumType)
        {
            string strconn = string.Empty;
            switch (connDbContextEnumType)
            {
                case ConnDbContextEnumType.Write:
                    strconn = _configuration.GetConnectionString("ConnectionStrings:Write");
                    break;
                case ConnDbContextEnumType.Rdad:
                    strconn = _configuration.GetConnectionString("ConnectionStrings:Rrite");
                    break;
                default:
                    break;
            }
            return null;
            return new UserDbContext(strconn);
        }
    }
}
