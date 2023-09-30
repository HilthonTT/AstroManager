using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Library.Api.Interfaces;
public interface IPasswordBreacherEndpoint
{
    Task<string> GeneratePasswordAsync(int length = 20);
    Task<List<CredentialModel>> GetBreachedCredentialsAsync(List<CredentialModel> credentials);
}