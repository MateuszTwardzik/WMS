using MagazynApp.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MagazynApp.Controllers
{
    [Authorize (Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserManagerController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be find";
                return View("NotFound");
            }
            var userViewModel = new ManageUserViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(userViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(ManageUserViewModel modelUser)
        {
            var user = await _userManager.FindByIdAsync(modelUser.UserId);
            if (user == null)
            {
                return View("NotFound");
            }
            if (user.UserName != modelUser.UserName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, modelUser.UserName);
                if (!setUserNameResult.Succeeded)
                {
                    return View();
                }
            }
            if (user.Email != modelUser.Email)
            {
                var setUserEmailResult = await _userManager.SetEmailAsync(user, modelUser.Email);
                if (!setUserEmailResult.Succeeded)
                {
                    return View();
                }
            }
            if (user.PhoneNumber != modelUser.PhoneNumber)
            {
                var setUserPhone = await _userManager.SetPhoneNumberAsync(user, modelUser.PhoneNumber);
                if (!setUserPhone.Succeeded)
                {
                    return View();
                }
            }
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string userId)
        {
            ViewBag.userId = userId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string userId, string newPassword)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            if (newPassword != null)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, newPassword);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

    }
}
