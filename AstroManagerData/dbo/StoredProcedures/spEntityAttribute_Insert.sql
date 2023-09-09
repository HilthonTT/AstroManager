CREATE PROCEDURE [dbo].[spEntityAttribute_Insert]
	@UserId INT,
	@EntityId INT,
	@AttributeId INT,
	@Value NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[EntityAttribute] (
		[UserId],
		[EntityId],
		[AttributeId],
		[Value]
	)
	VALUES (
		@UserId,
		@EntityId,
		@AttributeId,
		@Value
	)

	RETURN 0;
END