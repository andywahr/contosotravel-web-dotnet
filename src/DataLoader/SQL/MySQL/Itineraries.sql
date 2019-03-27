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
    IdP VARCHAR(36)
)
BEGIN
    SELECT Lower(Replace(Id,'-', '')) as Id, DepartingFlight, ReturningFlight, CarReservation, CarReservationDuration, HotelReservation, HotelReservationDuration, RecordLocator, PurchasedOn FROM Itineraries
    WHERE Id = IdP;
END
GO

DROP PROCEDURE IF EXISTS GetItineraryByRecordLocatorId
GO
CREATE PROCEDURE GetItineraryByRecordLocatorId(
    RecordLocatorP varchar(10)
)
BEGIN
    SELECT Lower(Replace(Id,'-', '')) as Id, DepartingFlight, ReturningFlight, CarReservation, CarReservationDuration, HotelReservation, HotelReservationDuration, RecordLocator, PurchasedOn FROM Itineraries
    WHERE RecordLocator = RecordLocatorP;
END
GO

DROP PROCEDURE IF EXISTS UpsertItinerary
GO
CREATE PROCEDURE UpsertItinerary(
    IdP                        VARCHAR(36),
    DepartingFlightP           int  ,
    ReturningFlightP          int  ,
    CarReservationP           int  ,
    CarReservationDurationP    FLOAT  ,
    HotelReservationP          int  ,
    HotelReservationDurationP  int  ,
    RecordLocatorP             varchar(10),
    PurchasedOnP              TIMESTAMP
)
BEGIN
	INSERT INTO Itineraries (Id, DepartingFlight, ReturningFlight, CarReservation, CarReservationDuration, HotelReservation, HotelReservationDuration, RecordLocator, PurchasedOn)
	VALUES 
	   (IdP, DepartingFlightP, ReturningFlightP, CarReservationP, CarReservationDurationP, HotelReservationP, HotelReservationDurationP, RecordLocatorP, PurchasedOnP)
	ON DUPLICATE KEY UPDATE DepartingFlight = DepartingFlightP, ReturningFlight = ReturningFlightP, CarReservation = CarReservationP, CarReservationDuration = CarReservationDurationP, HotelReservation = HotelReservationP, HotelReservationDuration = HotelReservationDurationP, RecordLocator = RecordLocatorP, PurchasedOn = PurchasedOnP;  
END
GO