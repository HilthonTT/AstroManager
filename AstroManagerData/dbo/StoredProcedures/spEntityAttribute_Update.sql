CREATE PROCEDURE [dbo].[spEntityAttribute_Update]
	@Id INT,
	@Value NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[EntityAttribute]
	SET [Value] = @Value
	WHERE [Id] = @Id;

	RETURN 0;
END