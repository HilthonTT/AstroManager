CREATE PROCEDURE [dbo].[spMasterPassword_GetByUserId]
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		[Id], 
		[UserId], 
		[HashedPassword]
	FROM [dbo].[MasterPassword]
	WHERE [UserId] = @UserId;

	RETURN 0;
END