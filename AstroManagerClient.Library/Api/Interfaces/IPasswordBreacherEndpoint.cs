using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Library.Api.Interfaces;
public interface IPasswordBreacherEndpoint
{
    Task<List<CredentialModel>> GetBreachedCredentialsAsync(List<CredentialModel> credentials);
}