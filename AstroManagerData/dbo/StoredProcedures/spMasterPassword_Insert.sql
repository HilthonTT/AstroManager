CREATE PROCEDURE [dbo].[spMasterPassword_Insert]
	@UserId INT,
	@HashedPassword NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[MasterPassword] (
		[UserId],
		[HashedPassword]
	)
	VALUES (
		@UserId,
		@HashedPassword
	)

	RETURN 0;
END