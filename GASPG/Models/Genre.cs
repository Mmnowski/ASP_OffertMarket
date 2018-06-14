using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASPG.Models
{
    public class Genre
    {
        public string GenreId { get; set; }

        public string Name { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}
