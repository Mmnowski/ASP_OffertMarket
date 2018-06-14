using GASPG.Helpers.CustomValidators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GASPG.Models.ViewModel.DeveloperViewModels
{
    public class CreateDeveloperViewModel
    {
        public string DeveloperId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Capitalized]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
