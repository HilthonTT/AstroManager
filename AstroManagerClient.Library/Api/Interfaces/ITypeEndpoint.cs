using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Library.Api.Interfaces;
public interface ITypeEndpoint
{
    Task<TypeModel> CreateTypeAsync(TypeModel type);
    Task<List<TypeModel>> GetAllTypesAsync();
}