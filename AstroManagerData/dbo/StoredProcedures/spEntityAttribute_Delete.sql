CREATE PROCEDURE [dbo].[spEntityAttribute_Delete]
    @UserId INT,
    @CredentialType NVARCHAR(50),
    @EntityAttributeId INT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM [dbo].[EntityAttribute] AS EA
        INNER JOIN Entity AS E ON EA.EntityId = E.Id
        WHERE EA.UserId = @UserId
          AND E.EntityType = @CredentialType
          AND EA.Id = @EntityAttributeId
    )
    BEGIN
        DELETE FROM [dbo].[EntityAttribute]
        WHERE UserId = @UserId
          AND EntityId IN (SELECT [Id] FROM [dbo].[Entity] WHERE [EntityType] = @CredentialType)
          AND Id = @EntityAttributeId;

        PRINT 'Credential deleted successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Credential not found or you do not have permission to delete it.';
    END
END;