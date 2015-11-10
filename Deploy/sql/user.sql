DROP TABLE Users;
GO

CREATE TABLE Users (

	UserID	int IDENTITY NOT NULL PRIMARY KEY,
	Name			varchar(256)  NOT NULL,
	Passwd			varchar(256) NOT NULL,
	--время последней активности пользователя
	LastUpdate		datetime,
	Color			varchar(256)
)
GO
----------------------------------------------------------------------------------------
DROP PROCEDURE Logon;
GO

CREATE PROCEDURE Logon
@LoginName		varchar(256),
@Passwd			varchar(256),
@Color			varchar(256)
AS

--если учетка есть возвращаем 1
 IF (SELECT COUNT(Users.UserID)AS Loged  FROM Users WHERE Name=@LoginName AND Passwd=@Passwd)=1
	BEGIN
		--Обновляем цвет и время
		UPDATE Users
		SET LastUpdate=CURRENT_TIMESTAMP,Color=@Color
		WHERE Name=@LoginName AND Passwd=@Passwd

		SELECT 1 AS Loged
		RETURN
	END

--если логин уже есть а пароль не совпал возвращаем 0
IF (SELECT COUNT(Users.UserID)AS Loged  FROM Users WHERE Name=@LoginName)=1 
	BEGIN
		SELECT 0 AS Loged
		RETURN
	END

--ввели новые данные создаем учетку
INSERT Users (Name,Passwd,LastUpdate,Color) VALUES (@LoginName,@Passwd,CURRENT_TIMESTAMP,@Color);
	SELECT 1 AS Loged
	RETURN
GO
-------------------------------------------------------------------------------
DROP PROC GetOnlineUsers
GO

CREATE PROC GetOnlineUsers
AS
SELECT UserID,Name,LastUpdate,Color FROM Users 
	WHERE convert(timestamp ,CURRENT_TIMESTAMP-LastUpdate)/300<30
GO
-------------------------------------------------------------------------------------
DROP PROC UpdateUserTime
GO

CREATE PROC UpdateUserTime
@LoginName		varchar(256)
AS
	UPDATE Users
		SET LastUpdate=CURRENT_TIMESTAMP
		WHERE Name=@LoginName
GO

------------------------------------------------------------------------------------

DELETE FROM Users

EXEC Logon 'user2','2','#0000CC'

EXEC GetOnlineUsers

SELECT * FROM Users


SELECT LastUpdate FROM Users WHERE Name='user1'
	
SELECT COUNT(M.UserId) FROM Messages M JOIN Users U ON M.UserId=U.UserID WHERE Name='user1'
		GROUP  BY M.UserId
