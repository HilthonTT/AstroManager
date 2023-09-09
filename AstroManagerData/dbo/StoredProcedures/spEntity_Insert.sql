CREATE PROCEDURE [dbo].[spEntity_Insert]
	@EntityType NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Entity] (
		[EntityType]
	)
	VALUES (
		@EntityType
	)

	RETURN 0;
END
