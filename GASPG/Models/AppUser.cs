using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASPG.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Game> Games { get; set; }
    }
}
