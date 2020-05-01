--@(#) script.ddl

CREATE TABLE City
(
	Name varchar (255),
	CityID integer IDENTITY(1,1),
	PRIMARY KEY(CityID)
);

CREATE TABLE GameType
(
	Name varchar (255),
	GameTypeID integer IDENTITY(1,1),
	IsOnline bit,
	PRIMARY KEY(GameTypeID)
);

CREATE TABLE [User]
(
	Username varchar (255),
	Email varchar (255),
	Firstname varchar (255),
	Lastname varchar (255),
	Birthdate date,
	ExternalID varchar (255),
	PictureURL varchar (255),
	UserID integer IDENTITY(1,1),
	PRIMARY KEY(UserID)
);

CREATE TABLE Location
(
	Latitude decimal,
	Longitude decimal,
	Address varchar (255),
	LocationID integer IDENTITY(1,1),
	fk_City integer NOT NULL,
	fk_GameType integer NOT NULL,
	PRIMARY KEY(LocationID),
	FOREIGN KEY(fk_City) REFERENCES City (CityID),
	FOREIGN KEY(fk_GameType) REFERENCES GameType (GameTypeID)
);

CREATE TABLE Team
(
	Name varchar (255),
	Type varchar (255),
	Description varchar (255),
	LogoURL varchar (255),
	TeamID integer IDENTITY(1,1),
	fk_Owner integer NOT NULL,
	fk_GameType integer NOT NULL,
	PRIMARY KEY(TeamID),
	FOREIGN KEY(fk_Owner) REFERENCES [User] (UserID),
	FOREIGN KEY(fk_GameType) REFERENCES GameType (GameTypeID)
);

CREATE TABLE Tournament
(
	Name varchar (255),
	Description varchar (255),
	MaxParticipantAmt int,
	Start datetime,
	TournamentID integer IDENTITY(1,1),
	ExternalID integer NOT NULL,
	TournamentUrl varchar(255) NOT NULL,
	fk_GameType integer NOT NULL,
	fk_Owner integer NOT NULL,
	PRIMARY KEY(TournamentID),
	FOREIGN KEY(fk_GameType) REFERENCES GameType (GameTypeID),
	FOREIGN KEY(fk_Owner) REFERENCES [User] (UserID)
);

CREATE TABLE Event
(
	Title varchar (255),
	MaxParticipantAmt int,
	StartTime datetime,
	EndTime datetime,
	IsPrivate bit NOT NULL DEFAULT 0,
	IsTeamEvent bit NOT NULL DEFAULT 0,
	EventID integer IDENTITY(1,1),
	fk_Owner integer NOT NULL,
	fk_Location integer NULL,
	fk_GameType integer NOT NULL,
	PRIMARY KEY(EventID),
	FOREIGN KEY(fk_Owner) REFERENCES [User] (UserID),
	FOREIGN KEY(fk_Location) REFERENCES Location (LocationID),
	FOREIGN KEY(fk_GameType) REFERENCES GameType (GameTypeID)
);

CREATE TABLE TeamInvitation
(
	IsAccepted bit NOT NULL DEFAULT 0,
	TeamInvitationID integer IDENTITY(1,1),
	fk_User integer NOT NULL,
	fk_Team integer NOT NULL,
	PRIMARY KEY(TeamInvitationID),
	FOREIGN KEY(fk_User) REFERENCES [User] (UserID),
	FOREIGN KEY(fk_Team) REFERENCES Team (TeamID)
);

CREATE TABLE TeamMember
(
	TeamMemberID integer IDENTITY(1,1),
	fk_User integer NOT NULL,
	fk_Team integer NOT NULL,
	PRIMARY KEY(TeamMemberID),
	FOREIGN KEY(fk_User) REFERENCES [User] (UserID),
	FOREIGN KEY(fk_Team) REFERENCES Team (TeamID)
);

CREATE TABLE TournamentMember
(
	TournamentMemberID integer IDENTITY(1,1),
	ExternalID integer NOT NULL,
	fk_Tournament integer NOT NULL,
	fk_Team integer NOT NULL,
	PRIMARY KEY(TournamentMemberID),
	FOREIGN KEY(fk_Tournament) REFERENCES Tournament (TournamentID),
	FOREIGN KEY(fk_Team) REFERENCES Team (TeamID)
);

CREATE TABLE EventInvitation
(
	IsAccepted bit NOT NULL DEFAULT 0,
	EventInvitationID integer IDENTITY(1,1),
	fk_User integer NOT NULL,
	fk_Event integer NOT NULL,
	PRIMARY KEY(EventInvitationID),
	FOREIGN KEY(fk_User) REFERENCES [User] (UserID),
	FOREIGN KEY(fk_Event) REFERENCES Event (EventID)
);

CREATE TABLE Participant
(
	ParticipantID integer IDENTITY(1,1),
	fk_User integer NOT NULL,
	fk_Event integer NOT NULL,
	fk_Team integer NOT NULL,
	PRIMARY KEY(ParticipantID),
	FOREIGN KEY(fk_User) REFERENCES [User] (UserID),
	FOREIGN KEY(fk_Event) REFERENCES Event (EventID),
	FOREIGN KEY(fk_Team) REFERENCES Team (TeamID)
);