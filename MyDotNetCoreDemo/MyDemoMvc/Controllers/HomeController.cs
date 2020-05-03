using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DemoFarmWork.MyFilter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDemoMvc.Models;

namespace MyDemoMvc.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [TypeFilter(typeof(MyMVCExceptionFilterAttribute))]//不需要注册 如果用ServiceFilter 则需要注入   services.AddScoped(typeof(MyMVCExceptionFilterAttribute));//不是直接new  而是容器生成 就可以自动注入了
        //[TypeFilter(typeof(MyResourceFilterAttribute)), TypeFilter(typeof(MyMVCExceptionFilterAttribute))]
       // [TypeFilter(typeof(MyResourceFilterAttribute))]
       [MyResourceFilter]
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
