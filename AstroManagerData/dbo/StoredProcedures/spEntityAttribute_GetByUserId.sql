CREATE PROCEDURE [dbo].[spEntityAttribute_GetByUserId]
	@UserId INT
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
	WHERE [UserId] = @UserId;

	RETURN 0;
END