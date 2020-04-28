using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFarmWork.MyFilter
{
    /// <summary>
    /// 缓存的 
    /// </summary>
    public class MyResourceFilterAttribute : Attribute, IResourceFilter, IFilterMetadata
    {

        //这里可以用Redis 做缓存

        /// <summary>
        /// 动作之前
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
           
            //if 有缓存，直接返回缓存
            //string key = context.HttpContext.Request.Path;
            //if (CustomCache.ContainsKey(key))
            //{
            //    context.Result = CustomCache[key];//断路器--到Result生成了，但是Result还需要转换成Html
            //}
        }

        /// <summary>
        /// 动作之后
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            
            ////这个应该缓存起来
            //string key = context.HttpContext.Request.Path;
            //if (!CustomCache.ContainsKey(key))
            //{
            //    CustomCache.Add(key, context.Result);
            //}
        }
    }
}
