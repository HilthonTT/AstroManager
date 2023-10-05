namespace AstroManagerClient.Models;
public static class ModelConverter
{
    public static CredentialModel GetCredential(CredentialDisplayModel credentialDisplay)
    {
        var listOfFields = new List<FieldModel>();

        foreach (var field in credentialDisplay.Fields)
        {
            var newField = new FieldModel()
            {
                Name = field.Name,
                Value = field.Value,
            };

            listOfFields.Add(newField);
        }

        var credential = new CredentialModel()
        {
            Id = credentialDisplay.Id,
            Title = credentialDisplay.Title,
            Type = new TypeModel()
            {
                Id = credentialDisplay.Type.Id,
                Name = credentialDisplay.Type.Name,
                Description = credentialDisplay.Type.Name,
            },
            Fields = listOfFields,
            User = credentialDisplay.User,
            DateAdded = credentialDisplay.DateAdded,
            Favorited = credentialDisplay.Favorited,
            DateModified = DateTime.UtcNow,
        };

        return credential;
    }
}
