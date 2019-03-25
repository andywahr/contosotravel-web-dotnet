DELIMITER GO

DROP TABLE IF EXISTS Flights
GO
CREATE TABLE Flights (
    Id INT NOT NULL,
    DepartingFrom CHAR(3) NOT NULL,
    ArrivingAt CHAR(3) NOT NULL,
    DepartureTime TIMESTAMP NOT NULL,
    ArrivalTime TIMESTAMP NOT NULL,
    Duration TIME NOT NULL,
    Cost FLOAT NOT NULL,
    CONSTRAINT PK_Flights PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Flights_DepartingFrom FOREIGN KEY (DepartingFrom)
        REFERENCES Airports (AirportCode),
    CONSTRAINT FK_Flights_ArrivingAt FOREIGN KEY (ArrivingAt)
        REFERENCES Airports (AirportCode)
);
GO

DROP PROCEDURE IF EXISTS CreateFlight
GO

CREATE PROCEDURE CreateFlight(
    Id int,
    DepartingFrom CHAR(3),
    ArrivingAt CHAR(3),
    DepartureTime TIMESTAMP,
    ArrivalTime TIMESTAMP,
    Duration TIME,
    Cost FLOAT
)
BEGIN
    INSERT INTO FLIGHTS (Id, DepartingFrom, ArrivingAt, DepartureTime, ArrivalTime, Duration, Cost)
                VALUES (Id, DepartingFrom, ArrivingAt, DepartureTime, ArrivalTime, Duration, Cost);
END
GO

DROP PROCEDURE IF EXISTS FindFlightById
GO
CREATE PROCEDURE FindFlightById(
    Id int
)
BEGIN
    SELECT Id, DepartingFrom, ArrivingAt, DepartureTime, ArrivalTime, Duration, Cost FROM FLIGHTS
    WHERE Id = Id;
END
GO

DROP PROCEDURE IF EXISTS FindFlights
GO
CREATE PROCEDURE FindFlights(
    DepartingFrom CHAR(3),
    ArrivingAt CHAR(3),
    DesiredTime TIMESTAMP,
    SecondsOffset int
)
BEGIN
    SELECT Id, DepartingFrom, ArrivingAt, DepartureTime, ArrivalTime, Duration, Cost FROM FLIGHTS
    WHERE DepartingFrom = DepartingFrom AND ArrivingAt = ArrivingAt AND 
    DepartureTime between DateAdd(s, -1 * SecondsOffset, DesiredTime) AND DateAdd(s, SecondsOffset, DesiredTime);
END