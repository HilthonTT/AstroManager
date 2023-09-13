using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using Microsoft.Identity.Client;

namespace AstroManagerClient.MsalClient;
public static class UserVerifier
{
    public static async Task<UserModel> VerifyUserInformationAsync(IUserEndpoint userEndpoint, AuthenticationResult result)
    {
        var claims = PCAWrapper.AcquireClaims(result);

        string objectId = claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;

        if (string.IsNullOrWhiteSpace(objectId))
        {
            return default;
        }

        var loggedInUser = await userEndpoint.GetUserFromAuthAsync(objectId) ?? new();

        string firstName = claims.FirstOrDefault(c => c.Type.Contains("givenname"))?.Value;
        string lastName = claims.FirstOrDefault(c => c.Type.Contains("surname"))?.Value;
        string displayName = claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
        string email = claims.FirstOrDefault(c => c.Type.Contains("email"))?.Value;

        bool isDirty = false;

        if (objectId.Equals(loggedInUser.ObjectIdentifier) is false)
        {
            isDirty = true;
            loggedInUser.ObjectIdentifier = objectId;
        }

        if (firstName.Equals(loggedInUser.FirstName) is false)
        {
            isDirty = true;
            loggedInUser.FirstName = firstName[..150];
        }

        if (lastName.Equals(loggedInUser.LastName) is false)
        {
            isDirty = true;
            loggedInUser.LastName = lastName[..150];
        }

        if (displayName.Equals(loggedInUser.DisplayName) is false)
        {
            isDirty = true;
            loggedInUser.DisplayName = displayName[..100];
        }

        if (email.Equals(loggedInUser.EmailAddress) is false)
        {
            isDirty = true;
            loggedInUser.EmailAddress = email[..256];
        }

        if (isDirty)
        {
            if (string.IsNullOrWhiteSpace(loggedInUser.Id))
            {
                loggedInUser = await userEndpoint.CreateUserAsync(loggedInUser);
            }
            else
            {
                await userEndpoint.UpdateUserAsync(loggedInUser);
            }
        }

        return loggedInUser;
    }
}
