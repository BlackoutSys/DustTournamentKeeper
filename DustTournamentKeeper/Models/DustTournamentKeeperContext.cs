using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DustTournamentKeeper.Models
{
    public class DustTournamentKeeperContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DustTournamentKeeperContext()
        {
        }

        public DustTournamentKeeperContext(DbContextOptions<DustTournamentKeeperContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Block> Blocks { get; set; }
        public virtual DbSet<BoardType> BoardTypes { get; set; }
        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<Faction> Factions { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Round> Rounds { get; set; }
        public virtual DbSet<TournamentBoardType> TournamentBoardTypes { get; set; }
        public virtual DbSet<TournamentUser> TournamentUsers { get; set; }
        public virtual DbSet<Tournament> Tournaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=PPS026;Database=DustTournamentKeeper;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Block>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.Icon)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Slogan)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Blocks)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_Blocks_Games");
            });

            modelBuilder.Entity<BoardType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Club>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Slogan)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Faction>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BlockId).HasColumnName("BlockID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.Icon)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Slogan)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.Factions)
                    .HasForeignKey(d => d.BlockId)
                    .HasConstraintName("FK_Factions_Blocks");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Factions)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_Factions_Games");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Logo)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Slogan)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BoardTypeId).HasColumnName("BoardTypeID");

                entity.Property(e => e.Bpa).HasColumnName("BPA");

                entity.Property(e => e.Bpb).HasColumnName("BPB");

                entity.Property(e => e.PlayerAid).HasColumnName("PlayerAID");

                entity.Property(e => e.PlayerBid).HasColumnName("PlayerBID");

                entity.Property(e => e.RoundId).HasColumnName("RoundID");

                entity.Property(e => e.SoSa).HasColumnName("SoSA");

                entity.Property(e => e.SoSb).HasColumnName("SoSB");

                entity.Property(e => e.Spa).HasColumnName("SPA");

                entity.Property(e => e.Spb).HasColumnName("SPB");

                entity.HasOne(d => d.BoardType)
                    .WithMany(p => p.Matches)
                    .HasForeignKey(d => d.BoardTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Matches_BoardTypes");

                entity.HasOne(d => d.PlayerA)
                    .WithMany(p => p.MatchesPlayerA)
                    .HasForeignKey(d => d.PlayerAid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Matches_User_A");

                entity.HasOne(d => d.PlayerB)
                    .WithMany(p => p.MatchesPlayerB)
                    .HasForeignKey(d => d.PlayerBid)
                    .HasConstraintName("FK_Matches_User_B");

                entity.HasOne(d => d.Round)
                    .WithMany(p => p.Matches)
                    .HasForeignKey(d => d.RoundId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Matches_Rounds");
            });

            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<Round>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TournamentId).HasColumnName("TournamentID");

                entity.HasOne(d => d.Tournament)
                    .WithMany(p => p.RoundsNavigation)
                    .HasForeignKey(d => d.TournamentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rounds_Tournaments");
            });

            modelBuilder.Entity<TournamentBoardType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BoardTypeId).HasColumnName("BoardTypeID");

                entity.Property(e => e.TournamentId).HasColumnName("TournamentID");

                entity.HasOne(d => d.BoardType)
                    .WithMany(p => p.TournamentBoardTypes)
                    .HasForeignKey(d => d.BoardTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoardTypes_TournamentBoardTypes");

                entity.HasOne(d => d.Tournament)
                    .WithMany(p => p.TournamentBoardTypes)
                    .HasForeignKey(d => d.TournamentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoardTypes_Tournaments");
            });

            modelBuilder.Entity<TournamentUser>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BlockId).HasColumnName("BlockID");

                entity.Property(e => e.Bp).HasColumnName("BP");

                entity.Property(e => e.FactionId).HasColumnName("FactionID");

                entity.Property(e => e.Sp).HasColumnName("SP");

                entity.Property(e => e.TournamentId).HasColumnName("TournamentID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Block)
                    .WithMany(p => p.TournamentUsers)
                    .HasForeignKey(d => d.BlockId)
                    .HasConstraintName("FK_Users_Block");

                entity.HasOne(d => d.Faction)
                    .WithMany(p => p.TournamentUsers)
                    .HasForeignKey(d => d.FactionId)
                    .HasConstraintName("FK_Users_Factions");

                entity.HasOne(d => d.Tournament)
                    .WithMany(p => p.TournamentUsers)
                    .HasForeignKey(d => d.TournamentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Tournaments");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TournamentUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Users");
            });

            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Bploss)
                    .HasColumnName("BPLoss")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Bptie)
                    .HasColumnName("BPTie")
                    .HasDefaultValueSql("((2))");

                entity.Property(e => e.Bpwin)
                    .HasColumnName("BPWin")
                    .HasDefaultValueSql("((3))");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Club)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClubId).HasColumnName("ClubID");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.Property(e => e.FeeCurrency)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");

                entity.Property(e => e.Slogan)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SpecificRules)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ClubNavigation)
                    .WithMany(p => p.Tournaments)
                    .HasForeignKey(d => d.ClubId)
                    .HasConstraintName("FK_Tournaments_Clubs");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Tournaments)
                    .HasForeignKey(d => d.GameId)
                    .HasConstraintName("FK_Tournaments_Games");

                entity.HasOne(d => d.Organizer)
                    .WithMany(p => p.Tournaments)
                    .HasForeignKey(d => d.OrganizerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tournaments_Users");
            });

            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasOne(d => d.Club)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ClubId)
                    .HasConstraintName("FK_Users_Clubs");
            });
        }
    }
}
