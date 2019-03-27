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
    IdP int
)
BEGIN
    SELECT Id, DepartingFrom, ArrivingAt, DepartureTime, ArrivalTime, Duration, Cost FROM FLIGHTS
    WHERE Id = IdP;
END
GO

DROP PROCEDURE IF EXISTS FindFlights
GO
CREATE PROCEDURE FindFlights(
    DepartingFromP CHAR(3),
    ArrivingAtP CHAR(3),
    DesiredTimeP TIMESTAMP,
    SecondsOffsetP int
)
BEGIN
    SELECT Id, DepartingFrom, ArrivingAt, DepartureTime, ArrivalTime, Duration, Cost FROM FLIGHTS
    WHERE DepartingFrom = DepartingFromP AND ArrivingAt = ArrivingAtP AND 
    DepartureTime between DATE_SUB(DesiredTimeP, INTERVAL SecondsOffsetP SECOND) AND (DATE_ADD(DesiredTimeP, INTERVAL SecondsOffsetP SECOND));
END
GO