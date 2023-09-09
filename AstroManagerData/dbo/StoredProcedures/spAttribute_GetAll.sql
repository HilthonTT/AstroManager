CREATE PROCEDURE [dbo].[spAttribute_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[Id], 
		[AttributeName]
	FROM [dbo].[Attribute]

	RETURN 0;
END