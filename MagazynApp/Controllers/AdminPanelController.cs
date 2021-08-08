using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Controllers
{
    public class AdminPanelController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
