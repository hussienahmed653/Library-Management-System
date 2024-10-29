--CREATE DATABASE AND TABLES--
--DATABASE--
CREATE DATABASE LibraryManagementSystem

USE LibraryManagementSystem

			--TABLE USERTYPES--
CREATE TABLE UsersType
(
	Id INT PRIMARY KEY IDENTITY,
	Type VARCHAR(50) NOT NULL
)
-------------------------------------------------
-------------------------------------------------
			--TABEL USERS--
CREATE TABLE Users
(
	Id INT PRIMARY KEY,
	Username NVARCHAR(50) NOT NULL,
	Email NVARCHAR(50) NOT NULL,
	Password NVARCHAR(50) NOT NULL,
	Gender NVARCHAR(10) NOT NULL,
	DateOfBirth DATETIME2 NOT NULL,
	UserType INT FOREIGN KEY REFERENCES UsersType(Id)
)
-------------------------------------------------
-------------------------------------------------
			--CLASSIFICATION--
CREATE TABLE Classification
(
	Id INT PRIMARY KEY,
	Classification NVARCHAR(1000) NOT NULL
)
-------------------------------------------------
-------------------------------------------------
			--TABLE BOOKS--
CREATE TABLE Books
(
	Id INT PRIMARY KEY,
	Title NVARCHAR(100) NOT NULL,
	ClassificationId INT FOREIGN KEY REFERENCES Classification(Id),
	YearOfPublication DATETIME2 NOT NULL,
	Author NVARCHAR(100) NOT NULL                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
)
-------------------------------------------------
-------------------------------------------------
			--TABLE SAVES--
CREATE TABLE Saves
(
	Id INT PRIMARY KEY,
	UserId INT FOREIGN KEY REFERENCES Users(Id),
	BookId INT FOREIGN KEY REFERENCES Books(Id),
	SaveDate DATETIME2 DEFAULT GETDATE()
)
-------------------------------------------------
-------------------------------------------------
			--TABLE BORROWINGBOOKSTATUS--
CREATE TABLE BorrowingBookStatus
(
	Id INT PRIMARY KEY,
	Status NVARCHAR(50) NOT NULL
)
-------------------------------------------------
-------------------------------------------------
			--TABLE BORROWINGS--
CREATE TABLE Borrowings
(
	Id INT PRIMARY KEY,
	UserId INT FOREIGN KEY REFERENCES Users(Id),
	BookId INT FOREIGN KEY REFERENCES Books(Id),
	BorrowDate DATETIME2 DEFAULT GETDATE(),
	ReturnDate AS DATEADD(DAY, 7, BorrowDate),
	BookStatus INT FOREIGN KEY REFERENCES BorrowingBookStatus(Id),
	UserReturnDate DATETIME2 NULL
)
-------------------------------------------------
-------------------------------------------------
			--TABLE RESERVATION STATUS--
CREATE TABLE ReservationStatus
(
	Id INT PRIMARY KEY,
	Status NVARCHAR(100) NOT NULL
)
-------------------------------------------------
-------------------------------------------------
			--TABLE RESERVATION--
CREATE TABLE Reservation
(
	Id INT PRIMARY KEY,
	UserId INT FOREIGN KEY REFERENCES Users(Id),
	BookId INT FOREIGN KEY REFERENCES Books(Id),
	ReservationsDate DATETIME2 DEFAULT GETDATE(),
	ExpiryDate AS DATEADD(DAY, 12, ReservationsDate),
	StatusId INT FOREIGN KEY REFERENCES ReservationStatus(Id)
)
-------------------------------------------------
-------------------------------------------------
			--TABLE FAVORITS--
CREATE TABLE Favorites
(
	Id INT PRIMARY KEY,
	UserId INT FOREIGN KEY REFERENCES Users(Id),
	BookId INT FOREIGN KEY REFERENCES Books(Id)
)
						---------------------------------------------
									 --END OF TABLES--
						---------------------------------------------

--CREATING STORED PROCEDURES--
				--GET BY USERNAME--
				--CHECK IF THERE IS ALREADY USERNAME IN TABLE USERS OR NOT--
				--REGISTRATION--
CREATE PROC GetByUsername 
(
	@username NVARCHAR(50)
)
AS
BEGIN
	SELECT
		U.Id,
		U.Username,
		U.Email,
		U.Password,
		U.Gender,
		U.DateOfBirth,
		U.UserType,
		UT.Type
	FROM Users U
	LEFT JOIN UsersType UT
	ON U.UserType = UT.Id
	WHERE Username = @username
	FOR JSON PATH,WITHOUT_ARRAY_WRAPPER
END
-------------------------------------------------
-------------------------------------------------
				--GET BY EMAIL--
				--CHECK IF THE USER WHO TRIE TO REGISTER HIS EMAIL IS NOT EXISTS--
				--REGISTRATION--
CREATE PROC GetByEmail 
(
	@Email NVARCHAR(MAX)
)
AS
BEGIN	
	SELECT COUNT(1) FROM Users WHERE Email = @Email
END
-------------------------------------------------
-------------------------------------------------
				--HERE THE OPERATION TO INSERT USER AND UPDATE USER PASSWORD--
CREATE PROC OperationUsers
(
	@JsonString NVARCHAR(MAX)
)
AS
BEGIN
	DECLARE @Action NVARCHAR(50);
	SELECT @Action = Action
FROM OPENJSON(@JsonString) 
WITH
(
    Action NVARCHAR(50) 
);

	DECLARE @Id INT;
	SELECT @Id = MAX(Id)+1 FROM Users
-- Check if the action is 'Insert'
IF @Action = 'Insert'
	BEGIN
		-- Perform the INSERT operation
		INSERT INTO Users
		SELECT 
			@Id,
			JS.Username,
			JS.Email,
			JS.Password,
			JS.Gender,
			JS.DateOfBirth,
			JS.UserType
		FROM OPENJSON(@JsonString)
		WITH
		(
			Username NVARCHAR(MAX) ,
			Email NVARCHAR(MAX),
			Password NVARCHAR(MAX) ,
			Gender NVARCHAR(MAX),
			DateOfBirth DATETIME2,
			UserType INT 
		)  AS JS
	END
--END OF INSERT--
				--BEGIN OF UPDATE--
--CHECK IF THE ACTION IS 'UPDATE'
	DECLARE @Action2 NVARCHAR(50);
	SELECT @Action2 = Action
	FROM OPENJSON(@JsonString) 
	WITH
	(
		Action NVARCHAR(50) 
	);
IF @Action2 = 'Update'
	BEGIN
		UPDATE U
		SET
		U.Password = JS.NewPassword
		FROM Users U
		INNER JOIN OPENJSON(@JsonString)
		WITH
		(
			Username NVARCHAR(50),
			Password NVARCHAR(50),
			NewPassword NVARCHAR(50),
			ConfirmNewPassword NVARCHAR(50)
		) AS JS
		ON U.Password = JS.Password
		where U.Username = JS.Username
	END
END
-------------------------------------------------
-------------------------------------------------
			--GET ALL BOOKS--
CREATE PROC GetAllBooks
AS
BEGIN
	SELECT 
	B.Id,
	B.Title,
	B.ClassificationId,
	B.YearOfPublication,
	B.Author,
	CASE
		WHEN EXISTS (SELECT 1
						FROM Borrowings BR
						WHERE BR.BookId = B.Id AND BR.UserReturnDate IS NULL)
		THEN CAST(0 AS bit)
		ELSE CAST(1 AS bit)
		END AS IsValid,
	CASE
		WHEN NOT EXISTS(SELECT 1
						FROM Borrowings BR
						WHERE BR.BookId = B.Id AND DATEDIFF(DAY, BR.ReturnDate, GETDATE()) > 0)
		THEN CAST(DATEDIFF(DAY, GETDATE(), (SELECT TOP 1 BR.ReturnDate FROM Borrowings BR WHERE BR.BookId = B.Id AND BR.UserReturnDate IS NULL ORDER BY BR.ReturnDate DESC)) AS INT)
		ELSE 0
		END AS ValidIn
	FROM Books B
	FOR JSON PATH
END
-------------------------------------------------
-------------------------------------------------
			--GET BOOKS BY PARAMETER--
CREATE PROC GetAllBooksByParameter 
(
	@JsonString NVARCHAR(MAX)
)
AS
BEGIN
--I WILL DECLARE VARIABLE TO SET VALUE FROM JSONSTRING ON IT--
	DECLARE
		@Title NVARCHAR(MAX),
		@ClassificationId NVARCHAR(MAX),
		@YearOfPublication DATETIME2,
		@Author NVARCHAR(MAX)

	SELECT 
	@Title = JS.Title,
	@ClassificationId = JS.ClassificationId,
	@YearOfPublication = JS.YearOfPublication,
	@Author = JS.Author
	FROM OPENJSON(@JsonString)
	WITH
	(
		Title NVARCHAR(MAX),
		ClassificationId NVARCHAR(MAX),
		YearOfPublication DATETIME2,
		Author NVARCHAR(MAX)
	) AS JS
	SELECT 
		B.Id,
		B.Title,
		B.ClassificationId,
		B.YearOfPublication,
		B.Author,
			CASE
			WHEN EXISTS (	SELECT 1 
							FROM Borrowings BR 
							WHERE BR.BookId = B.Id AND BR.UserReturnDate IS NULL)
			THEN CAST(0 AS bit)
			ELSE CAST(1 AS bit)
			END AS IsValid
    FROM Books B
    WHERE (@Title IS NULL OR Title = @Title)
    AND (@ClassificationId IS NULL OR ClassificationId = @ClassificationId)
    AND (@YearOfPublication IS NULL OR YearOfPublication = @YearOfPublication)
    AND (@Author IS NULL OR Author = @Author)
    FOR JSON PATH;
END
-------------------------------------------------
-------------------------------------------------
			--OPERATOIN INSERT AND UPDATE BOOKS--
CREATE PROC OperationBooks
(
	@JsonString NVARCHAR(MAX)
)
AS
BEGIN
--DECLARE HOW MANY ROWS AFFECTED--
	DECLARE @RowAffect INT = 0
--DECLARE VARIABLE TO ID--
	DECLARE @Id INT
	SELECT @Id = MAX(Id)+1 FROM Books
--DECLARE INSERT ACTION VARIABLE--
	DECLARE @ACTION NVARCHAR(20)
	SELECT @ACTION = Action 
	FROM OPENJSON(@JsonString)
	WITH
	(
		Action NVARCHAR(20)
	)
	IF @ACTION = 'Insert'
	BEGIN
		INSERT INTO Books
		SELECT 
			@Id,
			js.Title,
			js.ClassificationId,
			js.YearOfPublication,
			js.Author
		FROM OPENJSON(@JsonString)
		WITH
		(
			Title NVARCHAR(1000),
			ClassificationId NVARCHAR(1000),
			YearOfPublication DATETIME2,
			Author NVARCHAR(1000)
		) AS js
		SET @RowAffect = @@ROWCOUNT
	END
--DECLARE UPDATE ACTION VARIABLE--
	DECLARE @Action2 NVARCHAR(20)
	SELECT @Action2 = Action
	FROM OPENJSON(@JsonString)
	WITH
	(
		Action NVARCHAR(20)
	)
	IF @Action2 = 'Update' 
	BEGIN
		UPDATE B
		SET
		B.Title = COALESCE(JS.Title, B.Title),
		B.ClassificationId = COALESCE(JS.ClassificationId, B.ClassificationId),
		B.YearOfPublication = COALESCE(JS.YearOfPublication, B.YearOfPublication),
		B.Author = COALESCE(JS.Author, B.Author)
		FROM Books B
		INNER JOIN OPENJSON(@JsonString)
		WITH
		(
			Id INT,
			Title NVARCHAR(MAX),
			ClassificationId NVARCHAR(MAX),
			YearOfPublication DATETIME2,
			Author NVARCHAR(MAX)
		) AS JS
		ON B.Id = JS.Id
		SET @RowAffect = @@ROWCOUNT
	END
	SELECT @RowAffect AS ROW
END
-------------------------------------------------
-------------------------------------------------
		--DELETE BOOKS--
Create PROC DeleteBook
(
	@UserId INT
)
AS
BEGIN
--DECLARE ROWCOUNT VARIABLE--
	DECLARE @ROWCOUNT INT = 0
	DELETE FROM Books WHERE Id = @UserId
	SET @ROWCOUNT = @@ROWCOUNT
	SELECT @ROWCOUNT
END
-------------------------------------------------
-------------------------------------------------
				--GET DATA FROM SAVES--
CREATE PROC GetDataFromSaves
(
	@JsonString NVARCHAR(MAX)
)
AS
BEGIN
	DECLARE @UserId INT
	SELECT @UserId = UserId
FROM OPENJSON(@JsonString)
WITH
(
	UserId INT
)
	SELECT *
	FROM Saves
	WHERE UserId = @UserId
	FOR JSON PATH
END
-------------------------------------------------
-------------------------------------------------

				--INSERT INTO TABLE SAVES--
CREATE PROC InsertIntoTableSaves
(
	@UserId INT,
	@BookId INT
)
AS
BEGIN
	DECLARE @ROWAFFECT INT = 0
	DECLARE @Id INT
	SELECT @Id = ISNULL(MAX(Id), 0) + 1 FROM Saves
	IF NOT EXISTS(SELECT 1 FROM Saves WHERE UserId = @UserId AND BookId = @BookId)
	BEGIN
		INSERT INTO Saves(Id, UserId, BookId) VALUES (@Id, @UserId, @BookId)
	END

	SET @ROWAFFECT = @@ROWCOUNT
	SELECT @ROWAFFECT AS ROW
END
-------------------------------------------------
-------------------------------------------------
			--INSERT INTO RESERVATIONS--
CREATE PROC InsertIntoTableReservation
(
	@UserId INT,
	@BookId INT
)
AS
BEGIN
	DECLARE @ROWAFFECT INT = 0
	DECLARE @Id INT
	SELECT @Id = ISNULL(MAX(Id), 0) + 1 FROM Reservation
	IF NOT EXISTS(SELECT 1 FROM Reservation WHERE UserId = @UserId AND BookId = @BookId)
	BEGIN
		INSERT INTO Reservation(Id, UserId, BookId) VALUES (@Id, @UserId, @BookId)
	END

	SET @ROWAFFECT = @@ROWCOUNT
	SELECT @ROWAFFECT AS ROW
END
-------------------------------------------------
-------------------------------------------------
			--DELETE FROM SAVES--
CREATE PROC DeleteFromTableSaves
(
	@UserId INT,
	@BookId INT
)
AS
BEGIN
	DECLARE @ROWAFFECT INT = 0
	DELETE FROM Saves WHERE UserId = @UserId AND BookId = @BookId
	SET @ROWAFFECT = @@ROWCOUNT
	SELECT @ROWAFFECT AS ROW
END
-------------------------------------------------
-------------------------------------------------
			--HERE I WILL GET TABLE BORROWINS--
CREATE PROC GetAllDataOfBorrowingsBook
AS
BEGIN
	UPDATE Borrowings
    SET BookStatus = CASE 
						WHEN UserReturnDate IS NULL AND DATEDIFF(DAY, BorrowDate, GETDATE()) > 30 THEN
							(SELECT Id FROM BorrowingBookStatus WHERE Status = 'Lost')
                        WHEN UserReturnDate IS NULL THEN 
                            (SELECT Id FROM BorrowingBookStatus WHERE Status = 'Borrowed') 
                        ELSE 
                            (SELECT Id FROM BorrowingBookStatus WHERE Status = 'Returned') 
					END
	SELECT *
	FROM Borrowings
	FOR JSON PATH
END
-------------------------------------------------
-------------------------------------------------
			--ADD BORROWING--
			--HERE WE RECORD A NEW BOOK BORROWIN FROM A SPECIFIC USER--
CREATE PROC AddBorrowing
( 
	@UserId INT,
	@BookId INT
)
AS
BEGIN
DECLARE @Id INT
SELECT @Id = ISNULL(MAX(Id), 0) + 1 FROM Borrowings 
DECLARE @ROWAFFECT INT = 0
	IF NOT EXISTS(SELECT 1 FROM Borrowings BR WHERE BR.BookId = @BookId AND BR.UserReturnDate IS NULL)
	BEGIN
		INSERT INTO Borrowings (Id, UserId, BookId, BorrowDate)
		VALUES (@Id, @UserId, @BookId, GETDATE())
	END
	SET @ROWAFFECT = @@ROWCOUNT 
	SELECT @ROWAFFECT AS Row
END
-------------------------------------------------
-------------------------------------------------
			--HERE WE RECORD THE DATE OF RETURNING THE BOOK TO THE LIBRARY--
CREATE PROCEDURE ReturnBook
(
	@JsonString VARCHAR(MAX)
)
AS
BEGIN
DECLARE @ROWAFFECT INT = 0
DECLARE @Action VARCHAR(50), @UserId INT, @BookId INT
	SELECT	@Action = Action,
			@UserId = UserId,
			@BookId = BookId
	FROM OPENjSON(@JsonString)
	WITH
	(
		Action VARCHAR(50),
		UserId INT,
		BookId INT
	) AS JS
	IF @Action = 'Update'
	BEGIN
		IF EXISTS(SELECT 1 FROM Borrowings BR WHERE BR.BookId = @BookId AND BR.UserReturnDate IS NULL)
		BEGIN
			UPDATE Borrowings 
			SET UserReturnDate = GETDATE()
			WHERE UserId = @UserId AND BookId = @BookId 
		END
		SET @ROWAFFECT = @@ROWCOUNT
		SELECT @ROWAFFECT AS ROW
	END
END
-------------------------------------------------
-------------------------------------------------
			--INSERT INTO RESERVATION--
CREATE PROC InsertIntoReservation 
(
	@UserId INT,
	@BookId INT
)
AS
BEGIN
	DECLARE @ROWAFFECT INT = 0
	DECLARE @Id INT
	SELECT @Id = ISNULL(MAX(Id), 0) + 1 FROM Reservation
	 IF EXISTS(
				SELECT 1 
				FROM Borrowings B
				WHERE B.BookId = @BookId 
				AND B.UserReturnDate IS NULL
				) 
		AND NOT EXISTS(
				SELECT 1
				FROM Reservation R
				WHERE R.BookId = @BookId
				)
	BEGIN
		
		INSERT INTO Reservation (Id, UserId, BookId, StatusId)
		VALUES (@Id, @UserId, @BookId, 1)
	END
	SET @ROWAFFECT = @@ROWCOUNT
	SELECT @ROWAFFECT AS ROW
END
-------------------------------------------------
-------------------------------------------------
			--GET ALL RESERVATION--
CREATE PROC GetAllReservation
AS
BEGIN
    -- Update Status to 'Cancelled' if reservation is older than 15 days
    UPDATE Reservation
    SET StatusId = (SELECT Id FROM ReservationStatus WHERE Status = 'Cancelled')
    WHERE DATEDIFF(DAY, ReservationsDate, GETDATE()) > 15 
      AND StatusId != (SELECT Id FROM ReservationStatus WHERE Status = 'Cancelled');

    -- Update Status to 'Completed' if the book is returned
    UPDATE Reservation
    SET StatusId = (SELECT Id FROM ReservationStatus WHERE Status = 'Completed')
    FROM Reservation R
    INNER JOIN Borrowings BO ON R.BookId = BO.BookId
    WHERE BO.UserReturnDate IS NOT NULL
      AND R.StatusId != (SELECT Id FROM ReservationStatus WHERE Status = 'Completed');

    -- Update Status to 'Pending' if the book is still reserved (not yet returned)
    UPDATE Reservation
    SET StatusId = (SELECT Id FROM ReservationStatus WHERE Status = 'Pending')
    FROM Reservation R
    INNER JOIN Borrowings BO ON R.BookId = BO.BookId
    WHERE BO.UserReturnDate IS NULL
      AND R.StatusId != (SELECT Id FROM ReservationStatus WHERE Status = 'Pending');

    -- Now retrieve and display the data
    SELECT 
        Id,
        UserId,
        BookId,
        ReservationsDate,
        ExpiryDate,
        StatusId
    FROM Reservation
    FOR JSON PATH
END
-------------------------------------------------
-------------------------------------------------
			--GET USER RESERVATION--
CREATE PROC GetUserReservation
(
	@JsonString NVARCHAR(MAX)
)
AS
BEGIN
DECLARE @UserId INT
SELECT @UserId = UserId 
FROM OPENJSON(@JsonString)
WITH
(
	UserId INT
)
    -- Update Status to 'Cancelled' if reservation is older than 15 days
    UPDATE Reservation
    SET StatusId = (SELECT Id FROM ReservationStatus WHERE Status = 'Cancelled')
    WHERE DATEDIFF(DAY, ReservationsDate, GETDATE()) > 15 
      AND StatusId != (SELECT Id FROM ReservationStatus WHERE Status = 'Cancelled');

    -- Update Status to 'Completed' if the book is returned
    UPDATE Reservation
    SET StatusId = (SELECT Id FROM ReservationStatus WHERE Status = 'Completed')
    FROM Reservation R
    INNER JOIN Borrowings BO ON R.BookId = BO.BookId
    WHERE BO.UserReturnDate IS NOT NULL
      AND R.StatusId != (SELECT Id FROM ReservationStatus WHERE Status = 'Completed');

    -- Update Status to 'Pending' if the book is still reserved (not yet returned)
    UPDATE Reservation
    SET StatusId = (SELECT Id FROM ReservationStatus WHERE Status = 'Pending')
    FROM Reservation R
    INNER JOIN Borrowings BO ON R.BookId = BO.BookId
    WHERE BO.UserReturnDate IS NULL
      AND R.StatusId != (SELECT Id FROM ReservationStatus WHERE Status = 'Pending');

    -- Now retrieve and display the data
    SELECT 
        Id,
        UserId,
        BookId,
        ReservationsDate,
        ExpiryDate,
        StatusId
    FROM Reservation
	WHERE UserId = @UserId
    FOR JSON PATH
END
-------------------------------------------------
-------------------------------------------------
			--INSERT INTO FAVORITES--
CREATE PROC InsertIntoFavorites
(
	@UserId INT,
	@BookId INT
)
AS
BEGIN
DECLARE @Id INT 
DECLARE @ROWAFFECT INT = 0
SELECT @Id = ISNULL(MAX(Id), 0) + 1 FROM Favorites
	INSERT INTO Favorites VALUES(@Id, @UserId, @BookId)
SET @ROWAFFECT = @@ROWCOUNT
SELECT @ROWAFFECT AS ROW
END
-------------------------------------------------
-------------------------------------------------
			--GET ALL USER FAVORITES--
CREATE PROC GetAllUserFavorites
(
	@JsonString NVARCHAR(MAX)
)
AS
BEGIN
	DECLARE @UserId INT
	SELECT @UserId = UserId 
	FROM OPENJSON(@JsonString)
	WITH
	(
		UserId INT
	)
	SELECT * 
	FROM Favorites 
	WHERE UserId = @UserId
	FOR JSON PATH
END
-------------------------------------------------
-------------------------------------------------
			--DELETE FROM FAVORITES--
CREATE PROC DeleteFromFavorites
(
	@UserId INT,
	@BookId INT
)
AS
BEGIN
	DECLARE @ROWAFFECT INT = 0
	DELETE FROM Favorites
	WHERE UserId = @UserId
	AND BookId = @BookId

	SET @ROWAFFECT = @@ROWCOUNT
	SELECT @ROWAFFECT AS ROW
END