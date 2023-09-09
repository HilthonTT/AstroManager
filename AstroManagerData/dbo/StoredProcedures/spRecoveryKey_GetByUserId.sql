CREATE PROCEDURE [dbo].[spRecoveryKey_GetByUserId]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[Id], 
		[UserId], 
		[HashedKey]
	FROM [dbo].[RecoveryKey]
	WHERE [UserId] = @UserId

	RETURN 0;
END