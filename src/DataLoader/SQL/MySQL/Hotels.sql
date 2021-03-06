DELIMITER GO

DROP TABLE IF EXISTS Hotels
GO

CREATE TABLE Hotels (
    Id INT NOT NULL,
    Location CHAR(3) NOT NULL,
    StartingTime TIMESTAMP NOT NULL,
    EndingTime TIMESTAMP NOT NULL,
    Cost FLOAT NOT NULL,
    RoomType INT NOT NULL,
    CONSTRAINT PK_Hotels PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Hotels_Location FOREIGN KEY (Location)
        REFERENCES Airports (AirportCode)
);
GO

DROP PROCEDURE IF EXISTS CreateHotel
GO

CREATE PROCEDURE CreateHotel(
    Id int,
    Location CHAR(3),
    StartingTime TIMESTAMP,
    EndingTime TIMESTAMP,
    Cost FLOAT,
    RoomType int
)
BEGIN
    INSERT INTO HOTELS (Id, Location, StartingTime, EndingTime, Cost, RoomType)
                VALUES (Id, Location, StartingTime, EndingTime, Cost, RoomType);
END
GO
DROP PROCEDURE IF EXISTS FindHotelById
GO
CREATE PROCEDURE FindHotelById(
    IdP int
)
BEGIN
    SELECT Id, Location, StartingTime, EndingTime, Cost, RoomType FROM HOTELS
    WHERE Id = IdP;
END
GO

DROP PROCEDURE IF EXISTS FindHotels
GO
CREATE PROCEDURE FindHotels(
    LocationP CHAR(3),
    DesiredTimeP TIMESTAMP
)
BEGIN
    SELECT Id, Location, StartingTime, EndingTime, Cost, RoomType FROM HOTELS
    WHERE Location = LocationP AND DesiredTimeP between StartingTime AND EndingTime;
END
GO