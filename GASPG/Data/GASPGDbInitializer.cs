using GASPG.Helpers;
using GASPG.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GASPG.Data
{
    public class GASPGDbInitializer
    {
        private readonly GASPGDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public GASPGDbInitializer(GASPGDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void Seed()
        {
            // create database + apply migrations
            _context.Database.Migrate();

            // add example roles
            if (!_context.Roles.Any())
            {
                var roleNames = new[]
                {
                    RoleHelper.Administrator,
                    RoleHelper.Moderator,
                    RoleHelper.User
                };

                foreach (var roleName in roleNames)
                {
                    var role = new IdentityRole(roleName) { NormalizedName = RoleHelper.Normalize(roleName) };
                    _context.Roles.Add(role);
                }
            }

            // add administrator account
            if (!_context.AppUsers.Any())
            {
                const string userName = "admin@admin.com";
                const string userPass = "admin";

                var user = new AppUser { UserName = userName, Email = userName };
                _userManager.CreateAsync(user, userPass).Wait();
                _userManager.AddToRoleAsync(user, RoleHelper.Administrator).Wait();
            }

            _context.SaveChanges();
        }
    }
}
