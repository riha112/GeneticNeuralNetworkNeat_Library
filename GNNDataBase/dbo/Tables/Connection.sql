CREATE TABLE [dbo].[Connection]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[FromId] INT NOT NULL,
	[ToId] INT NOT NULL,
	[Weight] DECIMAL(18,17) DEFAULT 1,
	[InnovationId] INT NOT NULL,
    [NetworkId] INT NOT NULL,
    [Enabled] BIT NOT NULL DEFAULT 1,
	CONSTRAINT [FK_Conn_Net] FOREIGN KEY ([NetworkId]) REFERENCES [NET]([Id]), 
    CONSTRAINT [FK_Conn_Innov] FOREIGN KEY ([InnovationId]) REFERENCES [Innovation]([Id]),
)
