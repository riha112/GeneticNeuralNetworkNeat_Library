﻿CREATE TABLE [dbo].[Batch]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(32) NOT NULL,
	[Description] NVARCHAR(MAX) NULL,
	[Generation] INT NOT NULL DEFAULT 1,
	[CreationDate] DATETIME2 NOT NULL DEFAULT getutcdate(),
)

GO

CREATE TRIGGER [dbo].[DeleteBatch] 
ON [dbo].[Batch]
INSTEAD OF DELETE
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM [dbo].[Innovation] WHERE [BatchId] IN (SELECT [Id] FROM DELETED)
	DELETE FROM [dbo].[NET] WHERE [BatchId] IN (SELECT [Id] FROM DELETED)
	DELETE FROM [dbo].[Batch] WHERE [Id] IN (SELECT [Id] FROM DELETED)
END
GO