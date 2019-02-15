using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        IQueryable<UserToTournament> UsersToTournaments { get; }
        IQueryable<Game> Games { get; }
        IQueryable<BoardTypeToTournament> BoardTypeToTournaments { get; }


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

        void Add(UserToTournament userToTournament);
        void Update(UserToTournament userToTournament, UserToTournament newUserToTournament);
        void Delete(UserToTournament userToTournament);

        void Add(Game game);
        void Update(Game game, Game newGame);
        void Delete(Game game);

        void Add(BoardTypeToTournament boardTypeToTournament);
        void Update(BoardTypeToTournament boardTypeToTournament, BoardTypeToTournament newBoardTypeToTournament);
        void Delete(BoardTypeToTournament boardTypeToTournament);
    }
}
