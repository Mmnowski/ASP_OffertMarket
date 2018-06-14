using GASPG.Helpers.CustomValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GASPG.Models.ViewModel.GameViewModels
{
    public class EditGameViewModel
    {
        public string GameId { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public string GenreId { get; set; }

        [Required]
        [Display(Name = "Developer")]
        public string DeveloperId { get; set; }

        [Required]
        [PEGI]
        [Display(Name = "PEGI rating", Prompt = "XX+")]
        public string PEGI { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, 1_000_000.0, ErrorMessage = "Wage must be between 0.0 and 1000000.0")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public IEnumerable<Developer> Developers { get; set; }
    }
}
