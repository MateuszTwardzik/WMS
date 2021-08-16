using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagazynApp.Controllers
{
    public class SuppliesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
