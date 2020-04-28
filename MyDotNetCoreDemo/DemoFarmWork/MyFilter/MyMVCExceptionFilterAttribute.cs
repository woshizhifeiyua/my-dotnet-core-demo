using DemoFarmWork.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoFarmWork.MyFilter
{
    /// <summary>
    /// MVC错误filter
    /// </summary>
    public class MyMVCExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<MyMVCExceptionFilterAttribute> _logger;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        public MyMVCExceptionFilterAttribute(ILogger<MyMVCExceptionFilterAttribute> logger, IModelMetadataProvider modelMetadataProvider)
        {
            _logger = logger;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            try
            {
                //写入日志

                if (!context.ExceptionHandled)
                {
                    this._logger.LogError(context.Exception, "OnException错误  {0}", $"Query -{context.HttpContext.Request?.Path} Path{context.HttpContext.Request?.Path} Body--{context.HttpContext.Request?.Body} Method-- {context.HttpContext.Request.Method} QueryString--{context.HttpContext.Request?.QueryString} Host--{context.HttpContext.Request?.Host}  ");

                    //this._logger.LogError($"{context.HttpContext.Request.RouteValues["controller"]} is Error");
                    if (this.IsAjaxRequest(context.HttpContext.Request))//header看看是不是XMLHttpRequest
                    {

                        context.Result = new JsonResult(new MyMVCResponseResult
                        {
                            ActionStatus = "FAIL",
                            ErrorCode = 500,
                            ErrorInfo = "请求错误",
                            Result = context.Exception.Message

                        });
                        //    new JsonResult(new
                        //{
                        //    Result = false,
                        //    Msg = context.Exception.Message
                        //});//中断式---请求到这里结束了，不再继续Action
                    }
                    else
                    {
                        var result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
                        result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
                        result.ViewData.Add("Exception", context.Exception);
                        context.Result = result;
                    }
                    context.ExceptionHandled = true;
                }
            }
            catch (Exception ex)
            {

            }

        }
        private bool IsAjaxRequest(HttpRequest request)
        {
            string header = request.Headers["X-Requested-With"];
            return "XMLHttpRequest".Equals(header);
        }

    }
}
