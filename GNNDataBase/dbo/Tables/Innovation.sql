CREATE TABLE [dbo].[Innovation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[NodeFromId] INT NOT NULL,
	[NodeToId] INT NOT NULL,
    [Type] TINYINT NOT NULL DEFAULT 0,
	[BatchId] INT NOT NULL,
	[CreationDate] DATETIME2 NOT NULL DEFAULT getutcdate(),
	CONSTRAINT [FK_Innov_Batch] FOREIGN KEY ([BatchId]) REFERENCES [Batch]([Id]),
)


GO

CREATE TRIGGER [dbo].[DeleteInnov]
ON [dbo].[Innovation]
INSTEAD OF DELETE
AS
BEGIN
	SET NOCOUNT ON;
	DELETE FROM [dbo].[Connection] WHERE [InnovationId] IN (SELECT [Id] FROM DELETED)
	DELETE FROM [dbo].[Node] WHERE [InnovationId] IN (SELECT [Id] FROM DELETED)
	DELETE FROM [dbo].[Innovation] WHERE [Id] IN (SELECT [Id] FROM DELETED)
END