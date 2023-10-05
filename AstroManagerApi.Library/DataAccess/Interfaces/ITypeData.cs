namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface ITypeData
{
    Task<TypeModel> CreateTypeAsync(TypeModel type);
    Task<List<TypeModel>> GetAllTypesAsync();
}