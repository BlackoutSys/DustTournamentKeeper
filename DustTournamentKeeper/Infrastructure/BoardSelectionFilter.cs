using System.ComponentModel.DataAnnotations;

namespace DustTournamentKeeper.Infrastructure
{
    public class BoardSelectionFilter
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Count { get; set; }
    }
}
