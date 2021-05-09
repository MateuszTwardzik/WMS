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
using Microsoft.AspNetCore.Identity;
namespace MagazynApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly MagazynContext _context;
        // private readonly UserManager<IdentityUser> userManager;
        //private readonly SignInManager<IdentityUser> signInManager;

        public LoginController(MagazynContext context)
        // UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            // this.userManager = userManager;
            //this.signInManager = signInManager;
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        /*
        public async Task<IActionResult> Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser{UserName = model.Name};

                //var data = _db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                // var data = await _context.User.FirstOrDefaultAsync(s => s.Name.Equals(userName) && s.Password.Equals(userPassword));
                var result = await signInManager.PasswordSignInAsync(model.Name, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            // if (ModelState.IsValid)
            //  {
            //var f_password = userPassword;
            //var data = _db.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
            var data = await _context.User.FirstOrDefaultAsync(s => s.Name.Equals(user.Name) && s.Password.Equals(user.Password));
            if (data != null)
            {
                HttpContext.Session.SetString("Name", data.Name);
                HttpContext.Session.SetInt32("userId", data.Id);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Login failed";
                TempData["Message"] = "Login failed";
                return RedirectToAction("Login");
            }
            //  }
            // return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
