using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Library.Api.Interfaces;
public interface ICredentialTemplateEndpoint
{
    Task<CredentialTemplateModel> CreateTemplateAsync(CredentialTemplateModel template);
    Task<List<CredentialTemplateModel>> GetAllTemplatesAsync();
    Task<CredentialTemplateModel> GetTemplateAsync(string id);
}