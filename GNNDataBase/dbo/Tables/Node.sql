CREATE TABLE [dbo].[Node]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[NetworkId] INT NOT NULL,
	[InnovationId] INT NOT NULL, 
    CONSTRAINT [FK_Node_Net] FOREIGN KEY ([NetworkId]) REFERENCES [NET]([Id]), 
    CONSTRAINT [FK_Node_Innov] FOREIGN KEY ([InnovationId]) REFERENCES [Innovation]([Id]),
)
