CREATE DATABASE Soccer; 
GO
USE Soccer;
GO
       
CREATE TABLE Team (
    Id                  INT IDENTITY(1,1)   NOT NULL,
    Name                VARCHAR(50)             NOT NULL,
    Country             VARCHAR(50)             NOT NULL,
    PlayersQuantity     INT                 NOT NULL,
    Enabled             BIT            NOT NULL CONSTRAINT Default_Team_Enabled DEFAULT 1,

   CONSTRAINT PrimaryKey_Team PRIMARY KEY (Id),
   CONSTRAINT Check_Team_Name CHECK (LEN(TRIM(Name)) >= 2),
   CONSTRAINT Check_Team_Country CHECK (LEN(TRIM(Country)) >= 2),
    CONSTRAINT Check_Team_PlayersQuantity CHECK (PlayersQuantity BETWEEN 11 AND 22)
)
CREATE TABLE League (
    Id INT IDENTITY(1,1)  NOT NULL,
    Name VARCHAR(50)   NOT NULL, 
    Country VARCHAR(50)   NOT NULL,
    StartDate DATETIME  NOT NULL CONSTRAINT Default_League_StartDate DEFAULT GETDATE() ,
    EndDate DATETIME  NOT NULL CONSTRAINT Default_League_EndDate DEFAULT GETDATE() ,
    Enabled BIT NOT NULL CONSTRAINT Default_League_Enabled DEFAULT 1 ,

    CONSTRAINT PrimaryKey_League PRIMARY KEY (Id),
    CONSTRAINT Check_League_Name CHECK (LEN(TRIM(Name)) >= 2),
    CONSTRAINT Check_League_Country CHECK (LEN(TRIM(Country)) >= 2),
    CONSTRAINT Check_League_Dates CHECK (StartDate <= EndDate )
)

CREATE TABLE LeagueTeam (
    LeagueId INT NOT NULL,
    TeamId INT NOT NULL,
    
    CONSTRAINT PrimaryKey_LeagueIdTeamId PRIMARY KEY (LeagueId, TeamId),
   CONSTRAINT ForeignKey_LeagueTeam FOREIGN KEY (LeagueId) REFERENCES League (Id) ON DELETE CASCADE,
    CONSTRAINT ForeignKey_TeamId FOREIGN KEY (TeamId) REFERENCES Team (Id) ON DELETE CASCADE
)