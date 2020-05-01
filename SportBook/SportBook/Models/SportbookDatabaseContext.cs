using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SportBook.Models
{
    public partial class SportbookDatabaseContext : DbContext
    {
        public SportbookDatabaseContext()
        {
        }

        public SportbookDatabaseContext(DbContextOptions<SportbookDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventInvitation> EventInvitation { get; set; }
        public virtual DbSet<GameType> GameType { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Participant> Participant { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<TeamInvitation> TeamInvitation { get; set; }
        public virtual DbSet<TeamMember> TeamMember { get; set; }
        public virtual DbSet<Tournament> Tournament { get; set; }
        public virtual DbSet<TournamentMember> TournamentMember { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CityId)
                    .HasColumnName("CityID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.EventId)
                    .HasColumnName("EventID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.FkGameType).HasColumnName("fk_GameType");

                entity.Property(e => e.FkLocation).HasColumnName("fk_Location");

                entity.Property(e => e.FkOwner).HasColumnName("fk_Owner");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkGameTypeNavigation)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.FkGameType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Event__fk_GameTy__3C69FB99");

                entity.HasOne(d => d.FkLocationNavigation)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.FkLocation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Event__fk_Locati__3B75D760");

                entity.HasOne(d => d.FkOwnerNavigation)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.FkOwner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Event__fk_Owner__3A81B327");
            });

            modelBuilder.Entity<EventInvitation>(entity =>
            {
                entity.Property(e => e.EventInvitationId)
                    .HasColumnName("EventInvitationID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FkEvent).HasColumnName("fk_Event");

                entity.Property(e => e.FkUser).HasColumnName("fk_User");

                entity.HasOne(d => d.FkEventNavigation)
                    .WithMany(p => p.EventInvitation)
                    .HasForeignKey(d => d.FkEvent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventInvi__fk_Ev__4D94879B");

                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.EventInvitation)
                    .HasForeignKey(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EventInvi__fk_Us__4CA06362");
            });

            modelBuilder.Entity<GameType>(entity =>
            {
                entity.Property(e => e.GameTypeId)
                    .HasColumnName("GameTypeID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId)
                    .HasColumnName("LocationID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FkCity).HasColumnName("fk_City");

                entity.Property(e => e.FkGameType).HasColumnName("fk_GameType");

                entity.Property(e => e.Latitude).HasColumnType("decimal(8, 6)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(8, 6)");

                entity.HasOne(d => d.FkCityNavigation)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.FkCity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Location__fk_Cit__2D27B809");

                entity.HasOne(d => d.FkGameTypeNavigation)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.FkGameType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Location__fk_Gam__2E1BDC42");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.Property(e => e.ParticipantId)
                    .HasColumnName("ParticipantID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FkEvent).HasColumnName("fk_Event");

                entity.Property(e => e.FkTeam).HasColumnName("fk_Team");

                entity.Property(e => e.FkUser).HasColumnName("fk_User");

                entity.HasOne(d => d.FkEventNavigation)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.FkEvent)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__fk_Ev__5165187F");

                entity.HasOne(d => d.FkTeamNavigation)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.FkTeam)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__fk_Te__52593CB8");

                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.Participant)
                    .HasForeignKey(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Participa__fk_Us__5070F446");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.TeamId)
                    .HasColumnName("TeamID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FkGameType).HasColumnName("fk_GameType");

                entity.Property(e => e.FkOwner).HasColumnName("fk_Owner");

                entity.Property(e => e.LogoUrl)
                    .HasColumnName("LogoURL")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkGameTypeNavigation)
                    .WithMany(p => p.Team)
                    .HasForeignKey(d => d.FkGameType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team__fk_GameTyp__31EC6D26");

                entity.HasOne(d => d.FkOwnerNavigation)
                    .WithMany(p => p.Team)
                    .HasForeignKey(d => d.FkOwner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Team__fk_Owner__30F848ED");
            });

            modelBuilder.Entity<TeamInvitation>(entity =>
            {
                entity.Property(e => e.TeamInvitationId)
                    .HasColumnName("TeamInvitationID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FkTeam).HasColumnName("fk_Team");

                entity.Property(e => e.FkUser).HasColumnName("fk_User");

                entity.HasOne(d => d.FkTeamNavigation)
                    .WithMany(p => p.TeamInvitation)
                    .HasForeignKey(d => d.FkTeam)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeamInvit__fk_Te__412EB0B6");

                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.TeamInvitation)
                    .HasForeignKey(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeamInvit__fk_Us__403A8C7D");
            });

            modelBuilder.Entity<TeamMember>(entity =>
            {
                entity.Property(e => e.TeamMemberId)
                    .HasColumnName("TeamMemberID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FkTeam).HasColumnName("fk_Team");

                entity.Property(e => e.FkUser).HasColumnName("fk_User");

                entity.HasOne(d => d.FkTeamNavigation)
                    .WithMany(p => p.TeamMember)
                    .HasForeignKey(d => d.FkTeam)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeamMembe__fk_Te__44FF419A");

                entity.HasOne(d => d.FkUserNavigation)
                    .WithMany(p => p.TeamMember)
                    .HasForeignKey(d => d.FkUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TeamMembe__fk_Us__440B1D61");
            });

            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.Property(e => e.TournamentId)
                    .HasColumnName("TournamentID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FkGameType).HasColumnName("fk_GameType");

                entity.Property(e => e.FkOwner).HasColumnName("fk_Owner");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Start).HasColumnType("datetime");

                entity.HasOne(d => d.FkGameTypeNavigation)
                    .WithMany(p => p.Tournament)
                    .HasForeignKey(d => d.FkGameType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tournamen__fk_Ga__34C8D9D1");

                entity.HasOne(d => d.FkOwnerNavigation)
                    .WithMany(p => p.Tournament)
                    .HasForeignKey(d => d.FkOwner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tournamen__fk_Ow__35BCFE0A");
            });

            modelBuilder.Entity<TournamentMember>(entity =>
            {
                entity.Property(e => e.TournamentMemberId)
                    .HasColumnName("TournamentMemberID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FkTeam).HasColumnName("fk_Team");

                entity.Property(e => e.FkTournament).HasColumnName("fk_Tournament");

                entity.HasOne(d => d.FkTeamNavigation)
                    .WithMany(p => p.TournamentMember)
                    .HasForeignKey(d => d.FkTeam)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tournamen__fk_Te__48CFD27E");

                entity.HasOne(d => d.FkTournamentNavigation)
                    .WithMany(p => p.TournamentMember)
                    .HasForeignKey(d => d.FkTournament)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tournamen__fk_To__47DBAE45");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ExternalId)
                    .HasColumnName("ExternalID")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Firstname)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Lastname)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PictureUrl)
                    .HasColumnName("PictureURL")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
