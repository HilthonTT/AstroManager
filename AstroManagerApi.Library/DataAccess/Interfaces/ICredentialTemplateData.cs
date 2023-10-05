namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface ICredentialTemplateData
{
    Task<CredentialTemplateModel> CreateTemplateAsync(CredentialTemplateModel template);
    Task<List<CredentialTemplateModel>> GetAllTemplatesAsync();
    Task<CredentialTemplateModel> GetTemplateAsync(string id);
}