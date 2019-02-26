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

        public IQueryable<Block> Blocks => _context.Block;
        public IQueryable<BoardType> BoardTypes => _context.BoardType;
        public IQueryable<Club> Clubs => _context.Club;
        public IQueryable<Faction> Factions => _context.Faction;
        public IQueryable<Match> Matches => _context.Match;
        public IQueryable<Role> Roles => _context.Role;
        public IQueryable<Round> Rounds => _context.Round;
        public IQueryable<Tournament> Tournaments => _context.Tournament;
        public IQueryable<User> Users => _context.User;
        public IQueryable<UserToTournament> UsersToTournaments => _context.UserToTournament;
        public IQueryable<Game> Games => _context.Game;
        public IQueryable<BoardTypeToTournament> BoardTypeToTournaments => _context.BoardTypeToTournament;

        public void Add(Block block)
        {
            _context.Block.Add(block);
            _context.SaveChanges();
        }

        public void Add(BoardType board)
        {
            _context.BoardType.Add(board);
            _context.SaveChanges();
        }

        public void Add(Club club)
        {
            _context.Club.Add(club);
            _context.SaveChanges();
        }

        public void Add(Faction faction)
        {
            _context.Faction.Add(faction);
            _context.SaveChanges();
        }

        public void Add(Match match)
        {
            _context.Match.Add(match);
            _context.SaveChanges();
        }

        public void Add(Role role)
        {
            _context.Role.Add(role);
            _context.SaveChanges();
        }

        public void Add(Round round)
        {
            _context.Round.Add(round);
            _context.SaveChanges();
        }

        public void Add(Tournament tournament)
        {
            _context.Tournament.Add(tournament);
            _context.SaveChanges();
        }

        public void Add(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }

        public void Add(UserToTournament userToTournament)
        {
            _context.UserToTournament.Add(userToTournament);
            _context.SaveChanges();
        }

        public void Add(Game game)
        {
            _context.Game.Add(game);
            _context.SaveChanges();
        }

        public void Add(BoardTypeToTournament boardTypeToTournament)
        {
            _context.BoardTypeToTournament.Add(boardTypeToTournament);
            _context.SaveChanges();
        }

        public void Delete(Block block)
        {
            _context.Block.Remove(block);
            _context.SaveChanges();
        }

        public void Delete(BoardType board)
        {
            _context.BoardType.Remove(board);
            _context.SaveChanges();
        }

        public void Delete(Club club)
        {
            _context.Club.Remove(club);
            _context.SaveChanges();
        }

        public void Delete(Faction faction)
        {
            _context.Faction.Remove(faction);
            _context.SaveChanges();
        }

        public void Delete(Match match)
        {
            _context.Match.Remove(match);
            _context.SaveChanges();
        }

        public void Delete(Role role)
        {
            _context.Role.Remove(role);
            _context.SaveChanges();
        }

        public void Delete(Round round)
        {
            _context.Round.Remove(round);
            _context.SaveChanges();
        }

        public void Delete(Tournament tournament)
        {
            _context.Tournament.Remove(tournament);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.User.Remove(user);
            _context.SaveChanges();
        }

        public void Delete(UserToTournament userToTournament)
        {
            _context.UserToTournament.Remove(userToTournament);
            _context.SaveChanges();
        }

        public void Delete(Game game)
        {
            _context.Game.Remove(game);
            _context.SaveChanges();
        }

        public void Delete(BoardTypeToTournament boardTypeToTournament)
        {
            _context.BoardTypeToTournament.Remove(boardTypeToTournament);
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
            match.SoSa = newMatch.SoSa;
            match.SoSb = newMatch.SoSb;
            match.Spa = newMatch.Spa;
            match.Spb = newMatch.Spb;
            match.Status = newMatch.Status;

            _context.SaveChanges();
        }

        public void Update(Role role, Role newRole)
        {
            role.Description = newRole.Description;
            role.Name = newRole.Name;

            _context.SaveChanges();
        }

        public void Update(Round round, Round newRound)
        {
            round.Comment = newRound.Comment;
            round.Number = newRound.Number;
            round.TournamentId = newRound.TournamentId;

            _context.SaveChanges();
        }

        public void UpdateDeep(Round round, Round newRound)
        {
            round.Comment = newRound.Comment;
            round.Number = newRound.Number;
            round.TournamentId = newRound.TournamentId;

            var matches = round.Match.ToList();
            var newMatches = newRound.Match.ToList();

            for (var i = 0; i < round.Match.Count; i++)
            {
                var match = matches[i];
                var newMatch = newMatches[i];

                match.BoardNumber = newMatch.BoardNumber;
                match.BoardTypeId = newMatch.BoardTypeId;
                match.Bpa = newMatch.Bpa;
                match.Bpb = newMatch.Bpb;
                match.Spa = newMatch.Spa;
                match.Spa = newMatch.Spa;
                match.Spb = newMatch.Spb;
                match.SoSa = newMatch.SoSa;
                match.SoSb = newMatch.SoSb;
                match.PlayerAid = newMatch.PlayerAid;
                match.PlayerBid = newMatch.PlayerBid;
                match.Status = newMatch.Status;
            }

            round.Match = matches;

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

            tournament.LastModified = DateTime.UtcNow;

            for (var i = 0; i < tournament.Round.Count; i++)
            {
                tournament.Round[i].Comment = newTournament.Round[i].Comment;
                tournament.Round[i].Number = newTournament.Round[i].Number;

                for (var j = 0; j < tournament.Round[i].Match.Count; j++)
                {
                    tournament.Round[i].Match[j].BoardNumber = newTournament.Round[i].Match[j].BoardNumber;
                    tournament.Round[i].Match[j].BoardTypeId = newTournament.Round[i].Match[j].BoardTypeId;
                    tournament.Round[i].Match[j].Bpa = newTournament.Round[i].Match[j].Bpa;
                    tournament.Round[i].Match[j].Bpb = newTournament.Round[i].Match[j].Bpb;
                    tournament.Round[i].Match[j].Spa = newTournament.Round[i].Match[j].Spa;
                    tournament.Round[i].Match[j].Spa = newTournament.Round[i].Match[j].Spa;
                    tournament.Round[i].Match[j].Spb = newTournament.Round[i].Match[j].Spb;
                    tournament.Round[i].Match[j].SoSa = newTournament.Round[i].Match[j].SoSa;
                    tournament.Round[i].Match[j].SoSb = newTournament.Round[i].Match[j].SoSb;
                    tournament.Round[i].Match[j].PlayerAid = newTournament.Round[i].Match[j].PlayerAid;
                    tournament.Round[i].Match[j].PlayerBid = newTournament.Round[i].Match[j].PlayerBid;
                    tournament.Round[i].Match[j].Status = newTournament.Round[i].Match[j].Status;
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
            user.Nickname = newUser.Nickname;
            user.Surname = newUser.Surname;

            _context.SaveChanges();
        }

        public void Update(UserToTournament userToTournament, UserToTournament newUserToTournament)
        {
            userToTournament.BlockId = newUserToTournament.BlockId;
            userToTournament.BonusPoints = newUserToTournament.BonusPoints;
            userToTournament.Bp = newUserToTournament.Bp;
            userToTournament.FactionId = newUserToTournament.FactionId;
            userToTournament.PenaltyPoints = newUserToTournament.PenaltyPoints;
            userToTournament.SoS = newUserToTournament.SoS;
            userToTournament.Sp = newUserToTournament.Sp;
            userToTournament.TournamentId = newUserToTournament.TournamentId;
            userToTournament.UserId = newUserToTournament.UserId;

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

        public void Update(BoardTypeToTournament boardTypeToTournament, BoardTypeToTournament newBoardTypeToTournament)
        {
            boardTypeToTournament.BoardTypeId = newBoardTypeToTournament.BoardTypeId;
            boardTypeToTournament.Number = newBoardTypeToTournament.Number;
            boardTypeToTournament.TournamentId = newBoardTypeToTournament.TournamentId;

            _context.SaveChanges();
        }
    }
}
