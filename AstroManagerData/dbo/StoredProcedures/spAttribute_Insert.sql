CREATE PROCEDURE [dbo].[spAttribute_Insert]
	@AttributeName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Attribute] (
		[AttributeName]
	)
	VALUES (
		@AttributeName
	)

	RETURN 0;
END