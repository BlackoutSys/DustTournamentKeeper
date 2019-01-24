using Microsoft.EntityFrameworkCore;

namespace DustTournamentKeeper.Models
{
    public class DustTournamentKeeperContext : DbContext
    {
        public DustTournamentKeeperContext()
        {
        }

        public DustTournamentKeeperContext(DbContextOptions<DustTournamentKeeperContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Block> Block { get; set; }
        public virtual DbSet<BoardType> BoardType { get; set; }
        public virtual DbSet<BoardTypeToTournament> BoardTypeToTournament { get; set; }
        public virtual DbSet<Club> Club { get; set; }
        public virtual DbSet<Faction> Faction { get; set; }
        public virtual DbSet<Match> Match { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleToUser> RoleToUser { get; set; }
        public virtual DbSet<Round> Round { get; set; }
        public virtual DbSet<Tournament> Tournament { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=PPS026;Database=DustTournamentKeeper;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Block>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

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

            modelBuilder.Entity<BoardTypeToTournament>(entity =>
            {
                entity.ToTable("BoardType_To_Tournament");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BoardTypeId).HasColumnName("BoardTypeID");

                entity.Property(e => e.TournamentId).HasColumnName("TournamentID");

                entity.HasOne(d => d.BoardType)
                    .WithMany(p => p.BoardTypeToTournament)
                    .HasForeignKey(d => d.BoardTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoardType_To_Tournament_BoardType");

                entity.HasOne(d => d.Tournament)
                    .WithMany(p => p.BoardTypeToTournament)
                    .HasForeignKey(d => d.TournamentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoardType_To_Tournament_Tournament");
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
                    .WithMany(p => p.Faction)
                    .HasForeignKey(d => d.BlockId)
                    .HasConstraintName("FK_Faction_Block");
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

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BoardType)
                    .WithMany(p => p.Match)
                    .HasForeignKey(d => d.BoardTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Match_BoardType");

                entity.HasOne(d => d.PlayerA)
                    .WithMany(p => p.MatchPlayerA)
                    .HasForeignKey(d => d.PlayerAid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Match_User_A");

                entity.HasOne(d => d.PlayerB)
                    .WithMany(p => p.MatchPlayerB)
                    .HasForeignKey(d => d.PlayerBid)
                    .HasConstraintName("FK_Match_User_B");

                entity.HasOne(d => d.Round)
                    .WithMany(p => p.Match)
                    .HasForeignKey(d => d.RoundId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Match_Round");
            });

            modelBuilder.Entity<Role>(entity =>
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

            modelBuilder.Entity<RoleToUser>(entity =>
            {
                entity.ToTable("Role_To_User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleToUser)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_To_User_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RoleToUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Role_To_User_User");
            });

            modelBuilder.Entity<Round>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TournamentId).HasColumnName("TournamentID");

                entity.HasOne(d => d.Tournament)
                    .WithMany(p => p.Round)
                    .HasForeignKey(d => d.TournamentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Round_Tournament");
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

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.Property(e => e.DateEnd).HasColumnType("datetime");

                entity.Property(e => e.DateStart).HasColumnType("datetime");

                entity.Property(e => e.FeeCurrency)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.OrganizatorId).HasColumnName("OrganizatorID");

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
                    .WithMany(p => p.Tournament)
                    .HasForeignKey(d => d.ClubId)
                    .HasConstraintName("FK_Tournament_Club");

                entity.HasOne(d => d.Organizator)
                    .WithMany(p => p.Tournament)
                    .HasForeignKey(d => d.OrganizatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tournament_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
        }
    }
}
