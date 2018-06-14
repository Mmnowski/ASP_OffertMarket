using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GASPG.Models.ViewModel.GameViewModels
{
    public class GameViewModel
    {
        public string GameId { get; set; }

        public AppUser Author { get; set; }

        [Display(Name = "Genre")]
        public Genre Genre { get; set; }

        [Display(Name = "Developer")]
        public Developer Developer { get; set; }

        public DateTime Released { get; set; }

        [Display(Name = "PEGI+")]
        public string PEGI { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool CanEdit { get; set; }
    }
}
