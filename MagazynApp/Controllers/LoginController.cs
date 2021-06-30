using Microsoft.AspNetCore.Mvc;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagazynApp.Models;
using MagazynApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
namespace MagazynApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly MagazynContext _context;

        public LoginController(MagazynContext context)
        {
            _context = context;
        }

        //GET : User

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("userId") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                var data = await _context.User.FirstOrDefaultAsync(s => s.Name.Equals(user.Name) && s.Password.Equals(user.Password));

                if (data != null)
                {
                    HttpContext.Session.SetString("Name", data.Name);
                    HttpContext.Session.SetInt32("userId", data.Id);
                    HttpContext.Session.SetInt32("userPermission", data.Permission);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    TempData["Message"] = "Podałeś nieprawidłowe dane!";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
