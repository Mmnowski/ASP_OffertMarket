using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASPG.Models
{
    public class Game
    {
        public string GameId { get; set; }

        public AppUser Author { get; set; }

        public Genre Genre { get; set; }

        public Developer Developer { get; set; } 

        public DateTime Realeased { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PEGI { get; set; }

        public decimal Price { get; set; }

    }
}
