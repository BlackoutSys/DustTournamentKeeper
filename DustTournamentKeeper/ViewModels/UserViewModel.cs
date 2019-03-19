using DustTournamentKeeper.Infrastructure;
using DustTournamentKeeper.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DustTournamentKeeper.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }


        public string Club { get; set; }
        public int? ClubId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public List<UserHistoryViewModel> History { get; set; }
        public List<SelectListItem> ClubsAvailable { get; set; } = new List<SelectListItem>();

        public UserViewModel()
        {

        }

        public UserViewModel(User user)
        {
            Name = user.Name;
            UserName = user.UserName;
            Surname = user.Surname;
            Country = user.Country;
            City = user.City;
            Club = user?.Club?.Name;
            ClubId = user?.ClubId;
            Email = user?.Email;

            History = new List<UserHistoryViewModel>();
            foreach (var tournamentUser in user.TournamentUsers)
            {
                if (tournamentUser.Tournament.Status == nameof(TournamentStatus.Finished))
                {
                    History.Add(new UserHistoryViewModel(user.Id, tournamentUser.Tournament));
                }
            }
        }
    }
}
