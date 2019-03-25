DELIMITER GO

DROP TABLE IF EXISTS Itineraries
GO
CREATE TABLE Itineraries (
    Id VARCHAR(36) NOT NULL,
    DepartingFlight INT NULL,
    ReturningFlight INT NULL,
    CarReservation INT NULL,
    CarReservationDuration FLOAT NULL,
    HotelReservation INT NULL,
    HotelReservationDuration INT NULL,
    RecordLocator VARCHAR(10) NOT NULL,
    PurchasedOn TIMESTAMP NOT NULL,
    CONSTRAINT PK_Itineraries PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Itineraries_DepartingFlight FOREIGN KEY (DepartingFlight)
        REFERENCES Flights (Id),
    CONSTRAINT FK_Itineraries_ReturningFlight FOREIGN KEY (ReturningFlight)
        REFERENCES Flights (Id),
    CONSTRAINT FK_Itineraries_CarReservation FOREIGN KEY (CarReservation)
        REFERENCES Cars (Id),
    CONSTRAINT FK_Itineraries_HotelReservation FOREIGN KEY (HotelReservation)
        REFERENCES Hotels (Id)
);
GO

DROP PROCEDURE IF EXISTS GetItineraryById
GO
CREATE PROCEDURE GetItineraryById(
    Id VARCHAR(36)
)
BEGIN
    SELECT Lower(Replace(Id,'-', '')) as Id, DepartingFlight, ReturningFlight, CarReservation, CarReservationDuration, HotelReservation, HotelReservationDuration, RecordLocator, PurchasedOn FROM Itineraries
    WHERE Id = Id;
END
GO

DROP PROCEDURE IF EXISTS GetItineraryByRecordLocatorId
GO
CREATE PROCEDURE GetItineraryByRecordLocatorId(
    RecordLocator varchar(10)
)
BEGIN
    SELECT Lower(Replace(Id,'-', '')) as Id, DepartingFlight, ReturningFlight, CarReservation, CarReservationDuration, HotelReservation, HotelReservationDuration, RecordLocator, PurchasedOn FROM Itineraries
    WHERE RecordLocator = RecordLocator;
END
GO

DROP PROCEDURE IF EXISTS UpsertItinerary
GO
CREATE PROCEDURE UpsertItinerary(
    Id                        VARCHAR(36),
    DepartingFlight           int  ,
    ReturningFlight           int  ,
    CarReservation            int  ,
    CarReservationDuration    FLOAT  ,
    HotelReservation          int  ,
    HotelReservationDuration  int  ,
    RecordLocator             varchar(10),
    PurchasedOn               TIMESTAMP
)
BEGIN
	INSERT INTO Itineraries (Id, DepartingFlight, ReturningFlight, CarReservation, CarReservationDuration, HotelReservation, HotelReservationDuration, RecordLocator)
	VALUES 
	   (source.Id, source.DepartingFlight, source.ReturningFlight, source.CarReservation, source.CarReservationDuration, source.HotelReservation, source.HotelReservationDuration, source.RecordLocator)
	ON DUPLICATE KEY UPDATE DepartingFlight = source.DepartingFlight, ReturningFlight = source.ReturningFlight, CarReservation = source.CarReservation, CarReservationDuration = source.CarReservationDuration, HotelReservation = source.HotelReservation, HotelReservationDuration = source.HotelReservationDuration, RecordLocator = source.RecordLocator;  
END
GO