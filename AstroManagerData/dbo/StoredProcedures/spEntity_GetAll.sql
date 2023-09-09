CREATE PROCEDURE [dbo].[spEntity_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[Id], 
		[EntityType]
	FROM [dbo].[Entity]

	RETURN 0;
END