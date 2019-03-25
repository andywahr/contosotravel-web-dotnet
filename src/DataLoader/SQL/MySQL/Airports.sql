DELIMITER GO

DROP TABLE IF EXISTS Airports
GO
CREATE TABLE Airports 
(
    AirportCode CHAR(3) NOT NULL, -- Primary Key column
    State CHAR(2) NOT NULL,
    AirportName VARCHAR(255) NOT NULL,
    CityName VARCHAR(50) NOT NULL,
    TimeZone VARCHAR(50) NOT NULL,
    CONSTRAINT PK_Airports PRIMARY KEY CLUSTERED (AirportCode ASC)
);
GO

DROP PROCEDURE IF EXISTS CreateAirport
GO
CREATE PROCEDURE CreateAirport(
    IN AirportCode CHAR(3), 
    IN State CHAR(2),
    IN AirportName VARCHAR(255),
    IN CityName VARCHAR(50),
    IN TimeZone VARCHAR(50)
)
BEGIN
    INSERT INTO Airports (AirportCode, AirportName, State, CityName, TimeZone)
                VALUES (AirportCode, AirportName, State, CityName, TimeZone);
END
GO

DROP PROCEDURE IF EXISTS GetAllAirports
GO
CREATE PROCEDURE GetAllAirports()
BEGIN
    SELECT AirportCode, AirportName, State, CityName, TimeZone FROM Airports;
END
GO

DROP PROCEDURE IF EXISTS FindAirportByCode
GO
CREATE PROCEDURE FindAirportByCode(
    AirportCode CHAR(3)
)
BEGIN
    SELECT AirportCode, AirportName, State, CityName, TimeZone FROM Airports
    WHERE AirportCode = AirportCode;
END
GO