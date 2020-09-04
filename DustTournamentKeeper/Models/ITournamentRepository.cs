using System.Linq;

namespace DustTournamentKeeper.Models
{
    public interface ITournamentRepository
    {
        IQueryable<Block> Blocks { get; }
        IQueryable<BoardType> BoardTypes { get; }
        IQueryable<Club> Clubs { get; }
        IQueryable<Faction> Factions { get; }
        IQueryable<Match> Matches { get; }
        IQueryable<Role> Roles { get; }
        IQueryable<Round> Rounds { get; }
        IQueryable<Tournament> Tournaments { get; }
        IQueryable<User> Users { get; }
        IQueryable<TournamentUser> TournamentUsers { get; }
        IQueryable<Game> Games { get; }
        IQueryable<TournamentBoardType> TournamentBoardTypes { get; }

        void Add(Block block);
        void Update(Block block, Block newBlock);
        void Delete(Block block);

        void Add(BoardType board);
        void Update(BoardType board, BoardType newBoard);
        void Delete(BoardType board);

        void Add(Club club);
        void Update(Club club, Club newClub);
        void Delete(Club club);

        void Add(Faction faction);
        void Update(Faction faction, Faction newFaction);
        void Delete(Faction faction);

        void Add(Match match);
        void Update(Match match, Match newMatch);
        void Delete(Match match);

        void Add(Role role);
        void Update(Role role, Role newRole);
        void Delete(Role role);

        void Add(Round round);
        void Update(Round round, Round newRound);
        void Delete(Round round);

        void Add(Tournament tournament);
        void Update(Tournament tournament, Tournament newTournament);
        void Delete(Tournament tournament);

        void Add(User user);
        void Update(User user, User newUser);
        void Delete(User user);

        void Add(TournamentUser tournamentUser);
        void Update(TournamentUser tournamentUser, TournamentUser newTournamentUser);
        void Delete(TournamentUser tournamentUser);

        void Add(Game game);
        void Update(Game game, Game newGame);
        void Delete(Game game);

        void Add(TournamentBoardType tournamentBoardType);
        void Update(TournamentBoardType tournamentBoardType, TournamentBoardType newTournamentBoardType);
        void Delete(TournamentBoardType tournamentBoardType);

        void SaveContext();
    }
}
