CREATE DATABASE CharacterDB;
GO

CREATE TABLE Locations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Url NVARCHAR(500)
);
GO

CREATE TABLE Episodes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Url NVARCHAR(500)
);
GO

CREATE TABLE Characters (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Status NVARCHAR(50),
    Species NVARCHAR(50),
    Type NVARCHAR(50),
    Gender NVARCHAR(20),
    OriginId INT FOREIGN KEY REFERENCES Locations(Id),
    LocationId INT FOREIGN KEY REFERENCES Locations(Id),
    Image NVARCHAR(500),
    Episodes NVARCHAR(MAX), 
    Url NVARCHAR(500),
    Created DATETIME
);
GO

SELECT * FROM dbo.Locations;
SELECT * FROM dbo.Episodes;
SELECT * FROM dbo.Characters;

-- DROP TABLE Characters;
-- DROP TABLE Episodes;
-- DROP TABLE Locations;