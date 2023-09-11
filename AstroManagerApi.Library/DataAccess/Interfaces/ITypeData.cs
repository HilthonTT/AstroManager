using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface ITypeData
{
    Task CreateTypeAsync(TypeModel type);
    Task<List<TypeModel>> GetAllTypesAsync();
}