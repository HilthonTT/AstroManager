CREATE PROCEDURE [dbo].[spUser_Update]
	@Id INT,
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@EmailAddress NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[User]
	SET [FirstName] = @FirstName,
		[LastName] = @LastName,
		[EmailAddress] = @EmailAddress
	WHERE [Id] = @Id;

	RETURN 0;
END