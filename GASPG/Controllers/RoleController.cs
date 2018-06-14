using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GASPG.Data;
using GASPG.Helpers;
using GASPG.Models;
using GASPG.Models.ViewModel.RoleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GASPG.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class RoleController : Controller
    {
        private readonly GASPGDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(GASPGDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            var users = await _context.AppUsers.ToListAsync();

            var viewModel = users.Select(user =>
            {
                var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
                var userRoleId = _context.Roles.FirstOrDefault(role => role.NormalizedName == userRole).Id;
                var vm = new RoleViewModel
                {
                    AppUser = user,
                    AppUserId = user.Id,
                    RoleId = userRoleId,
                    Roles = roles,
                    Disabled = string.Equals(userRole, RoleHelper.Administrator)
                };
                return vm;
            });

            return View(viewModel);
        }

        // POST: Role/ChangeRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string applicationUserId, string roleId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            var user = _context.AppUsers.First(c => c.Id == applicationUserId);
            var newRole = _context.Roles.First(c => c.Id == roleId);

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, newRole.Name);

            return RedirectToAction(nameof(Index));
        }
    }
}