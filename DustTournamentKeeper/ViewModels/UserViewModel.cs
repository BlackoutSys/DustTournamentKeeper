using DustTournamentKeeper.Models;
using System.Collections.Generic;

namespace DustTournamentKeeper.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(User user)
        {
            Name = user.Name ?? "-";
            UserName = user.UserName ?? "-";
            Surname = user.Surname ?? "-";
            Country = user.Country ?? "-";
            City = user.City ?? "-";
            Club = user?.Club?.Name ?? "-";

            History = new List<UserHistoryViewModel>();
            foreach (var tournament in user.Tournaments)
            {
                if (tournament.Status == "Finished")
                {
                    History.Add(new UserHistoryViewModel(user.Id, tournament));
                }
            }
        }

        public string Name { get; set; }
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Club { get; set; }
        public string Email { get; set; }
        public List<UserHistoryViewModel> History { get; set; }

    }
}
