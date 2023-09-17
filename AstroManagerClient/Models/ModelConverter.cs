﻿using AstroManagerClient.Library.Models;

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
            Type = credentialDisplay.Type,
            Fields = listOfFields,
            Notes = credentialDisplay.Notes,
            User = credentialDisplay.User,
            DateAdded = credentialDisplay.DateAdded,
            DateModified = DateTime.UtcNow,
        };

        return credential;
    }
}