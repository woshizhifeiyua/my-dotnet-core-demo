using DemoDALUser.Model.Model;
using DemoFarmWork.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDALUser.Model
{
    public class UserDbContext : DbContext
    {

        //        Add-Migration Init  //其中Init是你的版本名称
        //update-database Init //更新数据库操作 init为版本名称
        //        Add-Migration EditPwdLength //同上，  update-database script EditPwdLength
        //update-database EditPwdLength   https://www.cnblogs.com/Fengge518/p/11446817.html

        private string _strconn = "";

        public UserDbContext(string strconn)
        {
            _strconn = strconn;


        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //这里初始化的时候需要注意
            optionsBuilder.UseSqlServer(_strconn);//数据库连接字符串，其中TestDB是数据库名称
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.Account).IsUnique();
            //user 为主 一对一
            //modelBuilder.Entity<User>().HasOne(u => u.Company).WithMany(u => u.UserInfos).HasForeignKey(u => u.CompanyId);
        }

        public DbSet<User> User { get; set; }

        public DbSet<Company> companies  { get; set; }

     
    }
}
