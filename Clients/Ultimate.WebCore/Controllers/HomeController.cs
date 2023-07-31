using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Exceptions;
using Ultimate.WebCore.Models;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICatalogService catalogService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            this.catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await catalogService.GetAllAsync());
        }

        public async Task<IActionResult> Detail(string id)
        {
            return View(await catalogService.GetByCourseIdAsync(id));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (errorFeature != null && errorFeature.Error is UnAuthorizeException)
            {
                return RedirectToAction(nameof(AuthController.LogOut), "Auth");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
