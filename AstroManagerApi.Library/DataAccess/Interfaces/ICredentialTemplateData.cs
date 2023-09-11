using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface ICredentialTemplateData
{
    Task CreateTemplateAsync(CredentialTemplateModel template);
    Task<List<CredentialTemplateModel>> GetAllTemplatesAsync();
    Task<CredentialTemplateModel> GetTemplateAsync(string id);
}