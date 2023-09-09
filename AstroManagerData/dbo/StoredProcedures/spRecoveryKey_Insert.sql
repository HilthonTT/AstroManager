CREATE PROCEDURE [dbo].[spRecoveryKey_Insert]
	@UserId INT,
	@HashedKey NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[RecoveryKey] (
		[UserId],
		[HashedKey]
	)
	VALUES (
		@UserId,
		@HashedKey
	)

	RETURN 0;
END