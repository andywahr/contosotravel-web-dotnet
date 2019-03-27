DELIMITER GO

DROP TABLE IF EXISTS Cars
GO
CREATE TABLE Cars (
    Id INT NOT NULL,
    Location CHAR(3) NOT NULL,
    StartingTime TIMESTAMP NOT NULL,
    EndingTime TIMESTAMP NOT NULL,
    Cost FLOAT NOT NULL,
    CarType INT NOT NULL,
    CONSTRAINT PK_Cars PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Cars_Location FOREIGN KEY (Location)
        REFERENCES Airports (AirportCode)
);
GO

DROP PROCEDURE IF EXISTS CreateCar
GO

CREATE PROCEDURE CreateCar(
    Id int,
    Location CHAR(3),
    StartingTime TIMESTAMP,
    EndingTime TIMESTAMP,
    Cost FLOAT,
    CarType int)
BEGIN
    INSERT INTO CARS (Id, Location, StartingTime, EndingTime, Cost, CarType)
                VALUES (Id, Location, StartingTime, EndingTime, Cost, CarType);
END
GO

DROP PROCEDURE IF EXISTS FindCarById
GO
CREATE PROCEDURE FindCarById(
    IdP int
)
BEGIN
    SELECT Id, Location, StartingTime, EndingTime, Cost, CarType FROM CARS
    WHERE Id = IdP;
END
GO

DROP PROCEDURE IF EXISTS FindCars
GO
CREATE PROCEDURE FindCars(
    LocationP CHAR(3),
    DesiredTimeP TIMESTAMP
)
BEGIN
    SELECT Id, Location, StartingTime, EndingTime, Cost, CarType FROM CARS
    WHERE Location = LocationP AND DesiredTimeP between StartingTime AND EndingTime;
END
GO