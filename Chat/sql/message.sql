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

SELECT * FROM Messages

EXEC [InsertMsg] 'user1','#000000','Test Message'