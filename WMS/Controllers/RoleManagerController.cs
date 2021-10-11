using MagazynApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MagazynApp.Controllers
{
    [Authorize (Roles = "Admin")]
    public class RoleManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IdentityContext _context;
        public RoleManagerController(RoleManager<IdentityRole> roleManager, IdentityContext context)
        {
            _roleManager = roleManager;
            _context = context;

        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if(roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            //var roles = await _roleManager.Roles.ToListAsync();
            //var role = _roleManager.Roles.First(r => r.Id.Equals(roleId));

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
            
            return RedirectToAction("Index");
        }
    }
}
