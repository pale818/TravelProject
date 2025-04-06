CREATE database Travel;

CREATE TABLE Destination (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Country NVARCHAR(100) NOT NULL
);

CREATE TABLE Trip (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    DateFrom DATE NOT NULL,
    DateTo DATE NOT NULL,
    Price DECIMAL(10,2),
    DestinationId INT FOREIGN KEY REFERENCES Destination(Id)
);

CREATE TABLE Guide (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Biography NVARCHAR(MAX),
    Email NVARCHAR(100) NOT NULL
);

-- bridge table between trip and Guide
CREATE TABLE TripGuide (
    TripId INT FOREIGN KEY REFERENCES Trip(Id),
    GuideId INT FOREIGN KEY REFERENCES Guide(Id),
    PRIMARY KEY (TripId, GuideId)
);

CREATE TABLE Wishlist (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES ApplicationUser(Id),
    TripId INT FOREIGN KEY REFERENCES Trip(Id)
);


CREATE TABLE Log (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES ApplicationUser(Id),
    Action NVARCHAR(100),
    Entity NVARCHAR(100),
    EntityId INT,
    Timestamp DATETIME DEFAULT GETDATE()
);

CREATE TABLE ApplicationUser (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    PhoneNumber NVARCHAR(20),
    IsAdmin BIT DEFAULT 0
);


-- checking jtw token and user
SELECT * FROM ApplicationUser;
SELECT * FROM ApplicationUser WHERE UserName = 'john';
DELETE FROM ApplicationUser WHERE UserName = 'john';


SELECT * FROM sys.tables;

USE Travel;
CREATE USER [DESKTOP-69AGKI8\Dario] FOR LOGIN [DESKTOP-69AGKI8\Dario];
ALTER ROLE db_owner ADD MEMBER [DESKTOP-69AGKI8\Dario];
USE master