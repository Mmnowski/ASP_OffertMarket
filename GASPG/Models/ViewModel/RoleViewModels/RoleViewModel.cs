using GASPG.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASPG.Models.ViewModel.RoleViewModels
{
    public class RoleViewModel
    {
        public AppUser AppUser { get; set; }

        public string AppUserId { get; set; }

        public string RoleId { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }

        public bool Disabled { get; set; }
    }
}
