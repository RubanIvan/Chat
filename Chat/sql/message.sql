DROP TABLE Messages
GO

CREATE TABLE Messages (

	MsgId			int IDENTITY NOT NULL PRIMARY KEY,
	[Time]			datetime,
	UserId			int,
	Color			varchar(256)  NOT NULL,
	Mesage			varchar(4096)  NOT NULL
)

GO

ALTER TABLE Messages  WITH CHECK ADD  CONSTRAINT [FK_Messages_UserID] FOREIGN KEY(UserID)
REFERENCES Users(UserID)
GO
----------------------------------------------------------------------------------------
DROP PROC InsertMsg
GO

CREATE PROC InsertMsg
@LoginName		varchar(256),
@Color			varchar(256),
@Msg			varchar(4096)
AS

declare @UserId int;
set @UserId=(SELECT UserID FROM Users Where Name=@LoginName)

	IF @UserId is null RETURN;

INSERT Messages (UserId,[Time],Color,Mesage) VALUES (@UserId,CURRENT_TIMESTAMP,@Color,@Msg);
GO

------------------------------------------------------------------------------------------
DROP PROC GetMessage
GO

CREATE PROC GetMessage
AS
--SELECT TOP(20) MsgId,Time,Name,M.Color,Mesage FROM Messages M JOIN Users U ON M.UserId=U.UserID ORDER BY MsgId DESC
SELECT * FROM [View] ORDER BY MsgId 
GO
-------------------------------------------------------------------------------------------
--получить все сообщения больше чем MessageId
DROP PROC GetLastMessage
GO
CREATE PROC GetLastMessage
@MessageId int
AS
SELECT MsgId,Time,Name,M.Color,Mesage FROM Messages M JOIN Users U ON M.UserId=U.UserID 
		 WHERE MsgId>@MessageId
		 ORDER BY MsgId 
GO
---------------------------------------------------------------------------------------------
SELECT * FROM Messages

EXEC [InsertMsg] 'user1','#0000cc','Test Message8'


SELECT (SELECT TOP(20) MsgId,Time,Name,M.Color,Mesage FROM Messages M JOIN Users U ON M.UserId=U.UserID ORDER BY MsgId DESC);

SELECT * FROM [View] ORDER BY MsgId 