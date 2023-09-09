using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IAttributeData
{
    Task CreateAttributeAsync(AttributeModel attribute);
    Task<List<AttributeModel>> GetAllAttributesAsync();
}