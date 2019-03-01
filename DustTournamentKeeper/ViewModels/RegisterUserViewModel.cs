using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DustTournamentKeeper.ViewModels
{
    public class RegisterUserViewModel
    {
        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Surname { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }
        public string Club { get; set; }
        public int ClubId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public List<SelectListItem> Clubs { get; set; }
    }
}
