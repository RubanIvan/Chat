CREATE TABLE Users (

	UserID	int IDENTITY NOT NULL PRIMARY KEY,
	Name			varchar(256)  NOT NULL,
	Passwd			varchar(256) NOT NULL
)
GO

DROP PROCEDURE Logon;
GO

CREATE PROCEDURE Logon
@LoginName		varchar(256),
@Passwd			varchar(256)
AS

--если учетка есть возвращаем 1
 IF (SELECT COUNT(Users.UserID)AS Loged  FROM Users WHERE Name=@LoginName AND Passwd=@Passwd)=1
	BEGIN
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
INSERT Users (Name,Passwd) VALUES (@LoginName,@Passwd);
	SELECT 1 AS Loged
	RETURN
GO


RETURN (SELECT 0 AS Loged)
EXEC Logon 'Test1','123'

SELECT * FROM Users
