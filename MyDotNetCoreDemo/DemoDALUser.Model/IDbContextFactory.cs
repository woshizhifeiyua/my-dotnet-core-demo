using DemoFarmWork.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDALUser.Model
{
    public interface IDbContextFactory
    {
        public UserDbContext CreateContext(ConnDbContextEnumType connDbContextEnumType);
    }
}
