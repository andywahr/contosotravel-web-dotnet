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
    Id VARCHAR(36)
)
BEGIN
    SELECT LOWER(Replace(id,'-', '')) as Id, DepartingFlight, ReturningFlight, CarReservation, CarReservationDuration, HotelReservation, HotelReservationDuration  FROM Carts
    WHERE Id = Id;
END
GO

DROP PROCEDURE IF EXISTS DeleteCart
GO
CREATE PROCEDURE DeleteCart(
    Id VARCHAR(36)
)
BEGIN
    DELETE FROM Carts
    WHERE Id = Id;
END
GO

DROP PROCEDURE IF EXISTS UpsertCartFlights
GO
CREATE PROCEDURE UpsertCartFlights(
    Id VARCHAR(36),
    DepartingFlight int,
    ReturningFlight int 
)
BEGIN
	INSERT INTO CARTS (Id, DepartingFlight, ReturningFlight)  
	VALUES 
	   (Id, DepartingFlight, ReturningFlight)
	ON DUPLICATE KEY UPDATE DepartingFlight = DepartingFlight, ReturningFlight = ReturningFlight;        
END
GO

DROP PROCEDURE IF EXISTS UpsertCartCar
GO
CREATE PROCEDURE UpsertCartCar(
    Id VARCHAR(36),
    CarReservation int,
    CarReservationDuration FLOAT  
)
BEGIN
	INSERT INTO Carts (Id, CarReservation, CarReservationDuration)  
	VALUES 
	   (Id, CarReservation, CarReservationDuration)
	ON DUPLICATE KEY UPDATE CarReservation = CarReservation, CarReservationDuration = CarReservationDuration;            
END
GO

DROP PROCEDURE IF EXISTS UpsertCartHotel
GO
CREATE PROCEDURE UpsertCartHotel(
    Id VARCHAR(36),
    HotelReservation int,
    HotelReservationDuration int
)
BEGIN
	INSERT INTO Carts (Id, HotelReservation, HotelReservationDuration)  
	VALUES 
	   (Id, HotelReservation, HotelReservationDuration)
	ON DUPLICATE KEY UPDATE HotelReservation = HotelReservation, HotelReservationDuration = HotelReservationDuration;      
END
GO