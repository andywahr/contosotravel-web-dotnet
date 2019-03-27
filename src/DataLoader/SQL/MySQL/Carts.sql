DELIMITER GO

DROP TABLE IF EXISTS Carts
GO
CREATE TABLE Carts
(
    Id                        VARCHAR(36) NOT NULL,
    DepartingFlight           int NULL,
    ReturningFlight           int NULL,
    CarReservation            int NULL,
    CarReservationDuration    FLOAT NULL,
    HotelReservation          int NULL,
    HotelReservationDuration  int NULL,
    CONSTRAINT PK_Carts PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Carts_DepartingFlight FOREIGN KEY (DepartingFlight) REFERENCES Flights (Id),
    CONSTRAINT FK_Carts_ReturningFlight FOREIGN KEY (ReturningFlight) REFERENCES Flights (Id),
    CONSTRAINT FK_Carts_CarReservation FOREIGN KEY (CarReservation) REFERENCES Cars (Id),
    CONSTRAINT FK_Carts_HotelReservation FOREIGN KEY (HotelReservation) REFERENCES Hotels (Id)
);
GO

DROP PROCEDURE IF EXISTS GetCartById
GO
CREATE PROCEDURE GetCartById(
    IdP VARCHAR(36)
)
BEGIN
    SELECT LOWER(Replace(id,'-', '')) as Id, DepartingFlight, ReturningFlight, CarReservation, CarReservationDuration, HotelReservation, HotelReservationDuration  FROM Carts
    WHERE Id = IdP;
END
GO

DROP PROCEDURE IF EXISTS DeleteCart
GO
CREATE PROCEDURE DeleteCart(
    IdP VARCHAR(36)
)
BEGIN
    DELETE FROM Carts
    WHERE Id = IdP;
END
GO

DROP PROCEDURE IF EXISTS UpsertCartFlights
GO
CREATE PROCEDURE UpsertCartFlights(
    IdP VARCHAR(36),
    DepartingFlightP int,
    ReturningFlightP int 
)
BEGIN
	INSERT INTO CARTS (Id, DepartingFlight, ReturningFlight)  
	VALUES 
	   (IdP, DepartingFlightP, ReturningFlightP)
	ON DUPLICATE KEY UPDATE DepartingFlight = DepartingFlightP, ReturningFlight = ReturningFlightP;        
END
GO

DROP PROCEDURE IF EXISTS UpsertCartCar
GO
CREATE PROCEDURE UpsertCartCar(
    IdP VARCHAR(36),
    CarReservationP int,
    CarReservationDurationP FLOAT  
)
BEGIN
	INSERT INTO Carts (Id, CarReservation, CarReservationDuration)  
	VALUES 
	   (IdP, CarReservationP, CarReservationDurationP)
	ON DUPLICATE KEY UPDATE CarReservation = CarReservationP, CarReservationDuration = CarReservationDurationP;            
END
GO

DROP PROCEDURE IF EXISTS UpsertCartHotel
GO
CREATE PROCEDURE UpsertCartHotel(
    IdP VARCHAR(36),
    HotelReservationP int,
    HotelReservationDurationP int
)
BEGIN
	INSERT INTO Carts (Id, HotelReservation, HotelReservationDuration)  
	VALUES 
	   (IdP, HotelReservationP, HotelReservationDurationP)
	ON DUPLICATE KEY UPDATE HotelReservation = HotelReservationP, HotelReservationDuration = HotelReservationDurationP;      
END
GO