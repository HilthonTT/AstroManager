using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Library.Api.Interfaces;
public interface ICredentialEndpoint
{
    Task<CredentialModel> CreateCredentialAsync(CredentialModel credential);
    Task<CredentialModel> GetCredentialAsync(string id);
    Task<List<CredentialModel>> GetUsersCredentialsAsync(string userId);
    Task<string> UpdateCredentialAsync(CredentialModel credential);
}