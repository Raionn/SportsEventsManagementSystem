CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Username] NVARCHAR(30) NOT NULL, 
    [Firstname] NVARCHAR(30) NULL, 
    [Lastname] NVARCHAR(30) NULL, 
    [Birthdate] DATE NULL, 
    [Password] NVARCHAR(200) NOT NULL, 
    [Email] NVARCHAR(50) NULL
)
