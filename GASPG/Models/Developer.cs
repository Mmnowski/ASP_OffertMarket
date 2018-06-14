using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASPG.Models
{
    public class Developer
    {
        public string DeveloperId{ get; set; }

        public string Name { get; set; }

        public int GameCount { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}
