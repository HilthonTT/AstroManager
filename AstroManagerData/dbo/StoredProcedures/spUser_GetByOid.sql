CREATE PROCEDURE [dbo].[spUser]
	@ObjectIdentifier NVARCHAR(36)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[Id], 
		[ObjectIdentifier], 
		[FirstName], 
		[LastName],
		[EmailAddress]
	FROM [dbo].[User]
	WHERE [ObjectIdentifier] = @ObjectIdentifier;

	RETURN 0;
END