CREATE PROCEDURE [dbo].[spUser_Insert]
	@ObjectIdentifier NVARCHAR(36),
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@EmailAddress NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[User] (
		[ObjectIdentifier],
		[FirstName],
		[LastName],
		[EmailAddress]
	)
	VALUES (
		@ObjectIdentifier,
		@FirstName,
		@LastName,
		@EmailAddress
	)

	RETURN 0;
END
