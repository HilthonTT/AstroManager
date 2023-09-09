CREATE PROCEDURE [dbo].[spMasterPassword_Update]
	@Id INT,
	@HashedPassword NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MasterPassword]
	SET [HashedPassword] = @HashedPassword
	WHERE [Id] = @Id;

	RETURN 0;
END