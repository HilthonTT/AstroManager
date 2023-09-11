using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface ICredentialData
{
    Task<CredentialModel> CreateCredentialAsync(CredentialModel credential);
    Task<List<CredentialModel>> GetAllCredentialsAsync();
    Task<CredentialModel> GetCredentialAsync(string id);
    Task<List<CredentialModel>> GetUsersCredentialsAsync(string userId);
    Task UpdateCredentialAsync(CredentialModel credential);
}