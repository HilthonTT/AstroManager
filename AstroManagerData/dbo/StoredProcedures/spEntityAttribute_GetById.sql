CREATE PROCEDURE [dbo].[spEntityAttribute_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[Id], 
		[UserId],
		[EntityId],
		[AttributeId], 
		[Value]
	FROM [dbo].[EntityAttribute]
	WHERE [Id] = @Id;
END