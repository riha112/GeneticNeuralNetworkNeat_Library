CREATE PROCEDURE [dbo].[GetInnovation]
	@Type int,
	@From int,
	@To int,
	@Batch int
AS
	IF (@Type = 0)
		SELECT * 
		FROM [dbo].[Innovation] 
		WHERE [NodeFromId]=@From AND [NodeToId]=@To AND [Type]=@Type AND [BatchId]=@Batch
	ELSE
		SELECT * 
		FROM [dbo].[Innovation]
		WHERE (([NodeFromId]=@From AND [NodeToId]=@To) OR ([NodeFromId]=@To AND [NodeToId]=@From)) AND [Type]=@Type AND [BatchId]=@Batch
RETURN
