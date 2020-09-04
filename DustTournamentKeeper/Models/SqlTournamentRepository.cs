using System;
using System.Linq;

namespace DustTournamentKeeper.Models
{
    public class SqlTournamentRepository : ITournamentRepository
    {
        private readonly DustTournamentKeeperContext _context;

        public SqlTournamentRepository(DustTournamentKeeperContext context)
        {
            _context = context;
        }

        public IQueryable<Block> Blocks => _context.Blocks;
        public IQueryable<BoardType> BoardTypes => _context.BoardTypes;
        public IQueryable<Club> Clubs => _context.Clubs;
        public IQueryable<Faction> Factions => _context.Factions;
        public IQueryable<Match> Matches => _context.Matches;
        public IQueryable<Role> Roles => _context.Roles;
        public IQueryable<Round> Rounds => _context.Rounds;
        public IQueryable<Tournament> Tournaments => _context.Tournaments;
        public IQueryable<User> Users => _context.Users;
        public IQueryable<TournamentUser> TournamentUsers => _context.TournamentUsers;
        public IQueryable<Game> Games => _context.Games;
        public IQueryable<TournamentBoardType> TournamentBoardTypes => _context.TournamentBoardTypes;

        public void Add(Block block)
        {
            _context.Blocks.Add(block);
            _context.SaveChanges();
        }

        public void Add(BoardType board)
        {
            _context.BoardTypes.Add(board);
            _context.SaveChanges();
        }

        public void Add(Club club)
        {
            _context.Clubs.Add(club);
            _context.SaveChanges();
        }

        public void Add(Faction faction)
        {
            _context.Factions.Add(faction);
            _context.SaveChanges();
        }

        public void Add(Match match)
        {
            _context.Matches.Add(match);
            _context.SaveChanges();
        }

        public void Add(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        public void Add(Round round)
        {
            _context.Rounds.Add(round);
            _context.SaveChanges();
        }

        public void Add(Tournament tournament)
        {
            tournament.Created = DateTime.Now;
            _context.Tournaments.Add(tournament);
            _context.SaveChanges();
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Add(TournamentUser tournamentUser)
        {
            _context.TournamentUsers.Add(tournamentUser);
            _context.SaveChanges();
        }

        public void Add(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
        }

        public void Add(TournamentBoardType tournamentBoardType)
        {
            _context.TournamentBoardTypes.Add(tournamentBoardType);
            _context.SaveChanges();
        }

        public void Delete(Block block)
        {
            _context.Blocks.Remove(block);
            _context.SaveChanges();
        }

        public void Delete(BoardType board)
        {
            _context.BoardTypes.Remove(board);
            _context.SaveChanges();
        }

        public void Delete(Club club)
        {
            _context.Clubs.Remove(club);
            _context.SaveChanges();
        }

        public void Delete(Faction faction)
        {
            _context.Factions.Remove(faction);
            _context.SaveChanges();
        }

        public void Delete(Match match)
        {
            _context.Matches.Remove(match);
            _context.SaveChanges();
        }

        public void Delete(Role role)
        {
            _context.Roles.Remove(role);
            _context.SaveChanges();
        }

        public void Delete(Round round)
        {
            _context.Rounds.Remove(round);
            _context.SaveChanges();
        }

        public void Delete(Tournament tournament)
        {
            _context.Tournaments.Remove(tournament);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public void Delete(TournamentUser tournamentUser)
        {
            _context.TournamentUsers.Remove(tournamentUser);
            _context.SaveChanges();
        }

        public void Delete(Game game)
        {
            _context.Games.Remove(game);
            _context.SaveChanges();
        }

        public void Delete(TournamentBoardType tournamentBoardType)
        {
            _context.TournamentBoardTypes.Remove(tournamentBoardType);
            _context.SaveChanges();
        }

        public void Update(Block block, Block newBlock)
        {
            block.GameId = newBlock.GameId;
            block.Icon = newBlock.Icon;
            block.Logo = newBlock.Icon;
            block.Name = newBlock.Name;
            block.Slogan = newBlock.Slogan;

            _context.SaveChanges();
        }

        public void Update(BoardType board, BoardType newBoard)
        {
            board.Description = newBoard.Description;
            board.Name = newBoard.Name;

            _context.SaveChanges();
        }

        public void Update(Club club, Club newClub)
        {
            club.Address = newClub.Address;
            club.City = newClub.City;
            club.Icon = newClub.Icon;
            club.Logo = newClub.Logo;
            club.Name = newClub.Name;
            club.Slogan = newClub.Slogan;

            _context.SaveChanges();
        }

        public void Update(Faction faction, Faction newFaction)
        {
            faction.BlockId = newFaction.BlockId;
            faction.GameId = newFaction.GameId;
            faction.Icon = newFaction.Icon;
            faction.Logo = newFaction.Logo;
            faction.Name = newFaction.Name;
            faction.Slogan = newFaction.Slogan;

            _context.SaveChanges();
        }

        public void Update(Match match, Match newMatch)
        {
            match.BoardTypeId = newMatch.BoardTypeId;
            match.Bpa = newMatch.Bpa;
            match.Bpb = newMatch.Bpb;
            match.PlayerAid = newMatch.PlayerAid;
            match.PlayerBid = newMatch.PlayerBid;
            match.RoundId = newMatch.RoundId;
            match.Spa = newMatch.Spa;
            match.Spb = newMatch.Spb;

            _context.SaveChanges();
        }

        public void Update(Role role, Role newRole)
        {
            role.Name = newRole.Name;

            _context.SaveChanges();
        }

        public void Update(Round round, Round newRound)
        {
            round.Number = newRound.Number;
            round.TournamentId = newRound.TournamentId;

            _context.SaveChanges();
        }

        public void Update(Tournament tournament, Tournament newTournament)
        {
            tournament.Address = newTournament.Address;
            tournament.ArmyPoints = newTournament.ArmyPoints;
            tournament.Bploss = newTournament.Bploss;
            tournament.Bptie = newTournament.Bptie;
            tournament.Bpwin = newTournament.Bpwin;
            tournament.City = newTournament.City;
            tournament.Club = newTournament.Club;
            tournament.ClubId = newTournament.ClubId;
            tournament.Country = newTournament.Country;
            tournament.DateStart = newTournament.DateStart;
            tournament.DateEnd = newTournament.DateEnd;
            tournament.Fee = newTournament.Fee;
            tournament.FeeCurrency = newTournament.FeeCurrency;
            tournament.GameId = newTournament.GameId;
            tournament.OrganizerId = newTournament.OrganizerId;
            tournament.PlayerLimit = newTournament.PlayerLimit;
            tournament.Rounds = newTournament.Rounds;
            tournament.Slogan = newTournament.Slogan;
            tournament.SpecificRules = newTournament.SpecificRules;
            tournament.Status = newTournament.Status;
            tournament.Title = newTournament.Title;
            tournament.TieBreaker1 = newTournament.TieBreaker1;
            tournament.TieBreaker2 = newTournament.TieBreaker2;
            tournament.TieBreaker3 = newTournament.TieBreaker3;
            tournament.TieBreaker4 = newTournament.TieBreaker4;


            tournament.LastModified = DateTime.Now;

            for (var i = 0; i < tournament.RoundsNavigation.Count; i++)
            {
                tournament.RoundsNavigation[i].Number = newTournament.RoundsNavigation[i].Number;

                for (var j = 0; j < tournament.RoundsNavigation[i].Matches.Count; j++)
                {
                    tournament.RoundsNavigation[i].Matches[j].BoardNumber = newTournament.RoundsNavigation[i].Matches[j].BoardNumber;
                    tournament.RoundsNavigation[i].Matches[j].BoardTypeId = newTournament.RoundsNavigation[i].Matches[j].BoardTypeId;
                    tournament.RoundsNavigation[i].Matches[j].Bpa = newTournament.RoundsNavigation[i].Matches[j].Bpa;
                    tournament.RoundsNavigation[i].Matches[j].Bpb = newTournament.RoundsNavigation[i].Matches[j].Bpb;
                    tournament.RoundsNavigation[i].Matches[j].Spa = newTournament.RoundsNavigation[i].Matches[j].Spa;
                    tournament.RoundsNavigation[i].Matches[j].Spa = newTournament.RoundsNavigation[i].Matches[j].Spa;
                    tournament.RoundsNavigation[i].Matches[j].Spb = newTournament.RoundsNavigation[i].Matches[j].Spb;
                    tournament.RoundsNavigation[i].Matches[j].SoSa = newTournament.RoundsNavigation[i].Matches[j].SoSa;
                    tournament.RoundsNavigation[i].Matches[j].SoSb = newTournament.RoundsNavigation[i].Matches[j].SoSb;
                    tournament.RoundsNavigation[i].Matches[j].PlayerAid = newTournament.RoundsNavigation[i].Matches[j].PlayerAid;
                    tournament.RoundsNavigation[i].Matches[j].PlayerBid = newTournament.RoundsNavigation[i].Matches[j].PlayerBid;
                }
            }

            _context.SaveChanges();
        }

        public void Update(User user, User newUser)
        {
            user.City = newUser.City;
            user.ClubId = newUser.ClubId;
            user.Country = newUser.Country;
            user.Email = newUser.Email;
            user.Name = newUser.Name;
            user.UserName = newUser.UserName;
            user.NormalizedUserName = newUser.UserName.ToUpper();
            user.Surname = newUser.Surname;
            user.LockoutEnd = newUser.LockoutEnd;

            _context.SaveChanges();
        }

        public void Update(TournamentUser tournamentUser, TournamentUser newTournamentUser)
        {
            _context.Attach(tournamentUser);

            tournamentUser.BlockId = newTournamentUser.BlockId;
            tournamentUser.FactionId = newTournamentUser.FactionId;

            _context.Entry(tournamentUser).Property(x => x.BlockId).IsModified = true;
            _context.Entry(tournamentUser).Property(x => x.FactionId).IsModified = true;

            _context.SaveChanges();
        }

        public void Update(Game game, Game newGame)
        {
            game.Description = newGame.Description;
            game.Icon = newGame.Icon;
            game.Logo = newGame.Logo;
            game.Name = newGame.Name;
            game.Slogan = newGame.Slogan;

            _context.SaveChanges();
        }

        public void Update(TournamentBoardType tournamentBoardType, TournamentBoardType newTournamentBoardType)
        {
            tournamentBoardType.BoardTypeId = newTournamentBoardType.BoardTypeId;
            tournamentBoardType.Number = newTournamentBoardType.Number;
            tournamentBoardType.TournamentId = newTournamentBoardType.TournamentId;

            _context.SaveChanges();
        }

        public void SaveContext()
        {
            _context.SaveChanges();
        }
    }
}
