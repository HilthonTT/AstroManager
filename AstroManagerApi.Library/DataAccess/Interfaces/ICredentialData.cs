using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface ICredentialData
{
    Task<CredentialModel> CreateCredentialAsync(CredentialModel credential);
    Task DeleteCredentialAsync(string id);
    Task<CredentialModel> GetCredentialAsync(string id);
    Task<List<CredentialModel>> GetUsersCredentialsAsync(string userId);
    Task UpdateCredentialAsync(CredentialModel credential);
}