using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportBook.Migrations
{
    public partial class TestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    CityID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.CityID);
                });

            migrationBuilder.CreateTable(
                name: "GameType",
                columns: table => new
                {
                    GameTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    IsOnline = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameType", x => x.GameTypeID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Firstname = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Lastname = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Birthdate = table.Column<DateTime>(type: "date", nullable: true),
                    ExternalID = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    PictureURL = table.Column<string>(unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<decimal>(type: "decimal(8, 6)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(8, 6)", nullable: false),
                    Address = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    fk_City = table.Column<int>(nullable: false),
                    fk_GameType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationID);
                    table.ForeignKey(
                        name: "FK__Location__fk_Cit__2D27B809",
                        column: x => x.fk_City,
                        principalTable: "City",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Location__fk_Gam__2E1BDC42",
                        column: x => x.fk_GameType,
                        principalTable: "GameType",
                        principalColumn: "GameTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    LogoURL = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    fk_Owner = table.Column<int>(nullable: false),
                    fk_GameType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamID);
                    table.ForeignKey(
                        name: "FK__Team__fk_GameTyp__31EC6D26",
                        column: x => x.fk_GameType,
                        principalTable: "GameType",
                        principalColumn: "GameTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Team__fk_Owner__30F848ED",
                        column: x => x.fk_Owner,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    TournamentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    MaxParticipantAmt = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(type: "datetime", nullable: false),
                    fk_GameType = table.Column<int>(nullable: false),
                    fk_Owner = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.TournamentID);
                    table.ForeignKey(
                        name: "FK__Tournamen__fk_Ga__34C8D9D1",
                        column: x => x.fk_GameType,
                        principalTable: "GameType",
                        principalColumn: "GameTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Tournamen__fk_Ow__35BCFE0A",
                        column: x => x.fk_Owner,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    MaxParticipantAmt = table.Column<int>(nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsPrivate = table.Column<bool>(nullable: false),
                    IsTeamEvent = table.Column<bool>(nullable: false),
                    fk_Owner = table.Column<int>(nullable: false),
                    fk_Location = table.Column<int>(nullable: true),
                    fk_GameType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventID);
                    table.ForeignKey(
                        name: "FK__Event__fk_GameTy__3C69FB99",
                        column: x => x.fk_GameType,
                        principalTable: "GameType",
                        principalColumn: "GameTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Event__fk_Locati__3B75D760",
                        column: x => x.fk_Location,
                        principalTable: "Location",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Event__fk_Owner__3A81B327",
                        column: x => x.fk_Owner,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamInvitation",
                columns: table => new
                {
                    TeamInvitationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAccepted = table.Column<bool>(nullable: false),
                    fk_User = table.Column<int>(nullable: false),
                    fk_Team = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamInvitation", x => x.TeamInvitationID);
                    table.ForeignKey(
                        name: "FK__TeamInvit__fk_Te__412EB0B6",
                        column: x => x.fk_Team,
                        principalTable: "Team",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TeamInvit__fk_Us__403A8C7D",
                        column: x => x.fk_User,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMember",
                columns: table => new
                {
                    TeamMemberID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fk_User = table.Column<int>(nullable: false),
                    fk_Team = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMember", x => x.TeamMemberID);
                    table.ForeignKey(
                        name: "FK__TeamMembe__fk_Te__44FF419A",
                        column: x => x.fk_Team,
                        principalTable: "Team",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__TeamMembe__fk_Us__440B1D61",
                        column: x => x.fk_User,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentMember",
                columns: table => new
                {
                    TournamentMemberID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fk_Tournament = table.Column<int>(nullable: false),
                    fk_Team = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentMember", x => x.TournamentMemberID);
                    table.ForeignKey(
                        name: "FK__Tournamen__fk_Te__48CFD27E",
                        column: x => x.fk_Team,
                        principalTable: "Team",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Tournamen__fk_To__47DBAE45",
                        column: x => x.fk_Tournament,
                        principalTable: "Tournament",
                        principalColumn: "TournamentID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventInvitation",
                columns: table => new
                {
                    EventInvitationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAccepted = table.Column<bool>(nullable: false),
                    fk_User = table.Column<int>(nullable: false),
                    fk_Event = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventInvitation", x => x.EventInvitationID);
                    table.ForeignKey(
                        name: "FK__EventInvi__fk_Ev__4D94879B",
                        column: x => x.fk_Event,
                        principalTable: "Event",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__EventInvi__fk_Us__4CA06362",
                        column: x => x.fk_User,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    ParticipantID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fk_User = table.Column<int>(nullable: false),
                    fk_Event = table.Column<int>(nullable: false),
                    fk_Team = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.ParticipantID);
                    table.ForeignKey(
                        name: "FK__Participa__fk_Ev__5165187F",
                        column: x => x.fk_Event,
                        principalTable: "Event",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Participa__fk_Te__52593CB8",
                        column: x => x.fk_Team,
                        principalTable: "Team",
                        principalColumn: "TeamID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Participa__fk_Us__5070F446",
                        column: x => x.fk_User,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_fk_GameType",
                table: "Event",
                column: "fk_GameType");

            migrationBuilder.CreateIndex(
                name: "IX_Event_fk_Location",
                table: "Event",
                column: "fk_Location");

            migrationBuilder.CreateIndex(
                name: "IX_Event_fk_Owner",
                table: "Event",
                column: "fk_Owner");

            migrationBuilder.CreateIndex(
                name: "IX_EventInvitation_fk_Event",
                table: "EventInvitation",
                column: "fk_Event");

            migrationBuilder.CreateIndex(
                name: "IX_EventInvitation_fk_User",
                table: "EventInvitation",
                column: "fk_User");

            migrationBuilder.CreateIndex(
                name: "IX_Location_fk_City",
                table: "Location",
                column: "fk_City");

            migrationBuilder.CreateIndex(
                name: "IX_Location_fk_GameType",
                table: "Location",
                column: "fk_GameType");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_fk_Event",
                table: "Participant",
                column: "fk_Event");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_fk_Team",
                table: "Participant",
                column: "fk_Team");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_fk_User",
                table: "Participant",
                column: "fk_User");

            migrationBuilder.CreateIndex(
                name: "IX_Team_fk_GameType",
                table: "Team",
                column: "fk_GameType");

            migrationBuilder.CreateIndex(
                name: "IX_Team_fk_Owner",
                table: "Team",
                column: "fk_Owner");

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvitation_fk_Team",
                table: "TeamInvitation",
                column: "fk_Team");

            migrationBuilder.CreateIndex(
                name: "IX_TeamInvitation_fk_User",
                table: "TeamInvitation",
                column: "fk_User");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_fk_Team",
                table: "TeamMember",
                column: "fk_Team");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_fk_User",
                table: "TeamMember",
                column: "fk_User");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_fk_GameType",
                table: "Tournament",
                column: "fk_GameType");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_fk_Owner",
                table: "Tournament",
                column: "fk_Owner");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMember_fk_Team",
                table: "TournamentMember",
                column: "fk_Team");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMember_fk_Tournament",
                table: "TournamentMember",
                column: "fk_Tournament");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventInvitation");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropTable(
                name: "TeamInvitation");

            migrationBuilder.DropTable(
                name: "TeamMember");

            migrationBuilder.DropTable(
                name: "TournamentMember");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Tournament");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "GameType");
        }
    }
}
