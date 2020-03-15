﻿CREATE TABLE [dbo].[Innovation]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[NodeFromId] INT NOT NULL,
	[NodeToId] INT NOT NULL,
    [Type] TINYINT NOT NULL DEFAULT 0,
	[BatchId] INT NOT NULL,
	[CreationDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
)
