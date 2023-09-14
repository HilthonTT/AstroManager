using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using Microsoft.Identity.Client;

namespace AstroManagerClient.MsalClient;
public static class UserVerifier
{
    public static async Task<UserModel> VerifyUserInformationAsync(IUserEndpoint userEndpoint, AuthenticationResult result)
    {
        var claims = PCAWrapper.AcquireClaims(result);

        string objectId = claims.FirstOrDefault(c => c.Type.Contains("oid"))?.Value;
        if (string.IsNullOrWhiteSpace(objectId))
        {
            return default;
        }

        var loggedInUser = await userEndpoint.GetUserFromAuthAsync(objectId) ?? new();
        
        string name = claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
        string firstName = "";
        string lastName = "";

        string[] nameParts = name.Split(' ');
        if (nameParts.Length >= 2)
        {
            firstName = nameParts[0];
            lastName = nameParts[1];
        }
        else
        {
            firstName = nameParts[0];
            lastName = "";
        }

        string displayName = claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
        string email = claims.FirstOrDefault(c => c.Type.Contains("preferred_username"))?.Value;

        bool isDirty = false;

        if (objectId.Equals(loggedInUser.ObjectIdentifier) is false)
        {
            isDirty = true;
            loggedInUser.ObjectIdentifier = objectId;
        }

        if (firstName.Equals(loggedInUser.FirstName) is false)
        {
            isDirty = true;
            loggedInUser.FirstName = firstName;
        }

        if (lastName.Equals(loggedInUser.LastName) is false)
        {
            isDirty = true;
            loggedInUser.LastName = lastName;
        }

        if (displayName.Equals(loggedInUser.DisplayName) is false)
        {
            isDirty = true;
            loggedInUser.DisplayName = displayName;
        }

        if (email.Equals(loggedInUser.EmailAddress) is false)
        {
            isDirty = true;
            loggedInUser.EmailAddress = email;
        }

        if (isDirty)
        {
            if (string.IsNullOrWhiteSpace(loggedInUser.Id))
            {
                await userEndpoint.CreateUserAsync(loggedInUser);
            }
            else
            {
                await userEndpoint.UpdateUserAsync(loggedInUser);
            }
        }

        return loggedInUser;
    }
}
