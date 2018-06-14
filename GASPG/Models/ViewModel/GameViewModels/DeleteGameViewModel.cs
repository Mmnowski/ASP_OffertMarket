using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASPG.Models.ViewModel.GameViewModels
{
    public class DeleteGameViewModel
    {
        public string GameId { get; set; }

        public AppUser Author { get; set; }

        public Genre Genre { get; set; }

        public Developer Developer { get; set; }

        public string Title { get; set; }

    }
}
