namespace AstroManagerApi.Data.Interfaces;

public interface IDummyDataService
{
    Task CreateDummyCredentialsAsync();
    Task CreateTemplatesAsync();
    Task CreateTypesAsync();
}