using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NuntaNoastra_Buta_Camelia.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NuntaNoastra_Buta_Camelia.Data;
using NuntaNoastra_Buta_Camelia.Models.ViewItem;

namespace NuntaNoastra_Buta_Camelia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopContext _context;
        public HomeController(ShopContext context)
        {
            _context = context;
        }
        /*private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

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
        public async Task<ActionResult> Statistics()
        {
            IQueryable<OrderGroup> data =
            from order in _context.Orders
            group order by order.OrderDate into dateGroup
            select new OrderGroup()
            {
                OrderDate = dateGroup.Key,
                CandleCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }
        public IActionResult Chat()
        {
            return View();
        }
    }
}
