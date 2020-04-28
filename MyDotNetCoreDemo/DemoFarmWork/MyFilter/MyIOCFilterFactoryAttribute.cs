using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFarmWork.MyFilter
{
    /// <summary>
    /// 自己实现IFilterFactory   Filter的依赖注入
    /// </summary>
    public class MyIOCFilterFactoryAttribute : Attribute, IFilterFactory
    {
        private readonly Type _FilterType = null;

        public MyIOCFilterFactoryAttribute(Type type)
        {
            this._FilterType = type;
        }
        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            //return (IFilterMetadata)serviceProvider.GetService(typeof(CustomExceptionFilterAttribute));

            return (IFilterMetadata)serviceProvider.GetService(this._FilterType);
        }
    }
}
