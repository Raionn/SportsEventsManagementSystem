﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SportBook.Models;

namespace SportBook.Migrations
{
    [DbContext(typeof(SportbookDatabaseContext))]
    [Migration("20200501134557_TestMigration")]
    partial class TestMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SportBook.Models.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("CityID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.HasKey("CityId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("SportBook.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EventID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("EndTime")
                        .IsRequired()
                        .HasColumnType("datetime");

                    b.Property<int>("FkGameType")
                        .HasColumnName("fk_GameType")
                        .HasColumnType("int");

                    b.Property<int?>("FkLocation")
                        .HasColumnName("fk_Location")
                        .HasColumnType("int");

                    b.Property<int>("FkOwner")
                        .HasColumnName("fk_Owner")
                        .HasColumnType("int");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTeamEvent")
                        .HasColumnType("bit");

                    b.Property<int?>("MaxParticipantAmt")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartTime")
                        .IsRequired()
                        .HasColumnType("datetime");

                    b.Property<string>("Title")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.HasKey("EventId");

                    b.HasIndex("FkGameType");

                    b.HasIndex("FkLocation");

                    b.HasIndex("FkOwner");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("SportBook.Models.EventInvitation", b =>
                {
                    b.Property<int>("EventInvitationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("EventInvitationID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FkEvent")
                        .HasColumnName("fk_Event")
                        .HasColumnType("int");

                    b.Property<int>("FkUser")
                        .HasColumnName("fk_User")
                        .HasColumnType("int");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.HasKey("EventInvitationId");

                    b.HasIndex("FkEvent");

                    b.HasIndex("FkUser");

                    b.ToTable("EventInvitation");
                });

            modelBuilder.Entity("SportBook.Models.GameType", b =>
                {
                    b.Property<int>("GameTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("GameTypeID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsOnline")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.HasKey("GameTypeId");

                    b.ToTable("GameType");
                });

            modelBuilder.Entity("SportBook.Models.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("LocationID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<int>("FkCity")
                        .HasColumnName("fk_City")
                        .HasColumnType("int");

                    b.Property<int>("FkGameType")
                        .HasColumnName("fk_GameType")
                        .HasColumnType("int");

                    b.Property<decimal?>("Latitude")
                        .IsRequired()
                        .HasColumnType("decimal(8, 6)");

                    b.Property<decimal?>("Longitude")
                        .IsRequired()
                        .HasColumnType("decimal(8, 6)");

                    b.HasKey("LocationId");

                    b.HasIndex("FkCity");

                    b.HasIndex("FkGameType");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("SportBook.Models.Participant", b =>
                {
                    b.Property<int>("ParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ParticipantID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FkEvent")
                        .HasColumnName("fk_Event")
                        .HasColumnType("int");

                    b.Property<int>("FkTeam")
                        .HasColumnName("fk_Team")
                        .HasColumnType("int");

                    b.Property<int>("FkUser")
                        .HasColumnName("fk_User")
                        .HasColumnType("int");

                    b.HasKey("ParticipantId");

                    b.HasIndex("FkEvent");

                    b.HasIndex("FkTeam");

                    b.HasIndex("FkUser");

                    b.ToTable("Participant");
                });

            modelBuilder.Entity("SportBook.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TeamID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<int>("FkGameType")
                        .HasColumnName("fk_GameType")
                        .HasColumnType("int");

                    b.Property<int>("FkOwner")
                        .HasColumnName("fk_Owner")
                        .HasColumnType("int");

                    b.Property<string>("LogoUrl")
                        .HasColumnName("LogoURL")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.HasKey("TeamId");

                    b.HasIndex("FkGameType");

                    b.HasIndex("FkOwner");

                    b.ToTable("Team");
                });

            modelBuilder.Entity("SportBook.Models.TeamInvitation", b =>
                {
                    b.Property<int>("TeamInvitationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TeamInvitationID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FkTeam")
                        .HasColumnName("fk_Team")
                        .HasColumnType("int");

                    b.Property<int>("FkUser")
                        .HasColumnName("fk_User")
                        .HasColumnType("int");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.HasKey("TeamInvitationId");

                    b.HasIndex("FkTeam");

                    b.HasIndex("FkUser");

                    b.ToTable("TeamInvitation");
                });

            modelBuilder.Entity("SportBook.Models.TeamMember", b =>
                {
                    b.Property<int>("TeamMemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TeamMemberID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FkTeam")
                        .HasColumnName("fk_Team")
                        .HasColumnType("int");

                    b.Property<int>("FkUser")
                        .HasColumnName("fk_User")
                        .HasColumnType("int");

                    b.HasKey("TeamMemberId");

                    b.HasIndex("FkTeam");

                    b.HasIndex("FkUser");

                    b.ToTable("TeamMember");
                });

            modelBuilder.Entity("SportBook.Models.Tournament", b =>
                {
                    b.Property<int>("TournamentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TournamentID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<int>("FkGameType")
                        .HasColumnName("fk_GameType")
                        .HasColumnType("int");

                    b.Property<int>("FkOwner")
                        .HasColumnName("fk_Owner")
                        .HasColumnType("int");

                    b.Property<int?>("MaxParticipantAmt")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<DateTime?>("Start")
                        .IsRequired()
                        .HasColumnType("datetime");

                    b.HasKey("TournamentId");

                    b.HasIndex("FkGameType");

                    b.HasIndex("FkOwner");

                    b.ToTable("Tournament");
                });

            modelBuilder.Entity("SportBook.Models.TournamentMember", b =>
                {
                    b.Property<int>("TournamentMemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("TournamentMemberID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FkTeam")
                        .HasColumnName("fk_Team")
                        .HasColumnType("int");

                    b.Property<int>("FkTournament")
                        .HasColumnName("fk_Tournament")
                        .HasColumnType("int");

                    b.HasKey("TournamentMemberId");

                    b.HasIndex("FkTeam");

                    b.HasIndex("FkTournament");

                    b.ToTable("TournamentMember");
                });

            modelBuilder.Entity("SportBook.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UserID")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("ExternalId")
                        .HasColumnName("ExternalID")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Firstname")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Lastname")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("PictureUrl")
                        .HasColumnName("PictureURL")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.Property<string>("Username")
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255)
                        .IsUnicode(false);

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SportBook.Models.Event", b =>
                {
                    b.HasOne("SportBook.Models.GameType", "FkGameTypeNavigation")
                        .WithMany("Event")
                        .HasForeignKey("FkGameType")
                        .HasConstraintName("FK__Event__fk_GameTy__3C69FB99")
                        .IsRequired();

                    b.HasOne("SportBook.Models.Location", "FkLocationNavigation")
                        .WithMany("Event")
                        .HasForeignKey("FkLocation")
                        .HasConstraintName("FK__Event__fk_Locati__3B75D760");

                    b.HasOne("SportBook.Models.User", "FkOwnerNavigation")
                        .WithMany("Event")
                        .HasForeignKey("FkOwner")
                        .HasConstraintName("FK__Event__fk_Owner__3A81B327")
                        .IsRequired();
                });

            modelBuilder.Entity("SportBook.Models.EventInvitation", b =>
                {
                    b.HasOne("SportBook.Models.Event", "FkEventNavigation")
                        .WithMany("EventInvitation")
                        .HasForeignKey("FkEvent")
                        .HasConstraintName("FK__EventInvi__fk_Ev__4D94879B")
                        .IsRequired();

                    b.HasOne("SportBook.Models.User", "FkUserNavigation")
                        .WithMany("EventInvitation")
                        .HasForeignKey("FkUser")
                        .HasConstraintName("FK__EventInvi__fk_Us__4CA06362")
                        .IsRequired();
                });

            modelBuilder.Entity("SportBook.Models.Location", b =>
                {
                    b.HasOne("SportBook.Models.City", "FkCityNavigation")
                        .WithMany("Location")
                        .HasForeignKey("FkCity")
                        .HasConstraintName("FK__Location__fk_Cit__2D27B809")
                        .IsRequired();

                    b.HasOne("SportBook.Models.GameType", "FkGameTypeNavigation")
                        .WithMany("Location")
                        .HasForeignKey("FkGameType")
                        .HasConstraintName("FK__Location__fk_Gam__2E1BDC42")
                        .IsRequired();
                });

            modelBuilder.Entity("SportBook.Models.Participant", b =>
                {
                    b.HasOne("SportBook.Models.Event", "FkEventNavigation")
                        .WithMany("Participant")
                        .HasForeignKey("FkEvent")
                        .HasConstraintName("FK__Participa__fk_Ev__5165187F")
                        .IsRequired();

                    b.HasOne("SportBook.Models.Team", "FkTeamNavigation")
                        .WithMany("Participant")
                        .HasForeignKey("FkTeam")
                        .HasConstraintName("FK__Participa__fk_Te__52593CB8")
                        .IsRequired();

                    b.HasOne("SportBook.Models.User", "FkUserNavigation")
                        .WithMany("Participant")
                        .HasForeignKey("FkUser")
                        .HasConstraintName("FK__Participa__fk_Us__5070F446")
                        .IsRequired();
                });

            modelBuilder.Entity("SportBook.Models.Team", b =>
                {
                    b.HasOne("SportBook.Models.GameType", "FkGameTypeNavigation")
                        .WithMany("Team")
                        .HasForeignKey("FkGameType")
                        .HasConstraintName("FK__Team__fk_GameTyp__31EC6D26")
                        .IsRequired();

                    b.HasOne("SportBook.Models.User", "FkOwnerNavigation")
                        .WithMany("Team")
                        .HasForeignKey("FkOwner")
                        .HasConstraintName("FK__Team__fk_Owner__30F848ED")
                        .IsRequired();
                });

            modelBuilder.Entity("SportBook.Models.TeamInvitation", b =>
                {
                    b.HasOne("SportBook.Models.Team", "FkTeamNavigation")
                        .WithMany("TeamInvitation")
                        .HasForeignKey("FkTeam")
                        .HasConstraintName("FK__TeamInvit__fk_Te__412EB0B6")
                        .IsRequired();

                    b.HasOne("SportBook.Models.User", "FkUserNavigation")
                        .WithMany("TeamInvitation")
                        .HasForeignKey("FkUser")
                        .HasConstraintName("FK__TeamInvit__fk_Us__403A8C7D")
                        .IsRequired();
                });

            modelBuilder.Entity("SportBook.Models.TeamMember", b =>
                {
                    b.HasOne("SportBook.Models.Team", "FkTeamNavigation")
                        .WithMany("TeamMember")
                        .HasForeignKey("FkTeam")
                        .HasConstraintName("FK__TeamMembe__fk_Te__44FF419A")
                        .IsRequired();

                    b.HasOne("SportBook.Models.User", "FkUserNavigation")
                        .WithMany("TeamMember")
                        .HasForeignKey("FkUser")
                        .HasConstraintName("FK__TeamMembe__fk_Us__440B1D61")
                        .IsRequired();
                });

            modelBuilder.Entity("SportBook.Models.Tournament", b =>
                {
                    b.HasOne("SportBook.Models.GameType", "FkGameTypeNavigation")
                        .WithMany("Tournament")
                        .HasForeignKey("FkGameType")
                        .HasConstraintName("FK__Tournamen__fk_Ga__34C8D9D1")
                        .IsRequired();

                    b.HasOne("SportBook.Models.User", "FkOwnerNavigation")
                        .WithMany("Tournament")
                        .HasForeignKey("FkOwner")
                        .HasConstraintName("FK__Tournamen__fk_Ow__35BCFE0A")
                        .IsRequired();
                });

            modelBuilder.Entity("SportBook.Models.TournamentMember", b =>
                {
                    b.HasOne("SportBook.Models.Team", "FkTeamNavigation")
                        .WithMany("TournamentMember")
                        .HasForeignKey("FkTeam")
                        .HasConstraintName("FK__Tournamen__fk_Te__48CFD27E")
                        .IsRequired();

                    b.HasOne("SportBook.Models.Tournament", "FkTournamentNavigation")
                        .WithMany("TournamentMember")
                        .HasForeignKey("FkTournament")
                        .HasConstraintName("FK__Tournamen__fk_To__47DBAE45")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
