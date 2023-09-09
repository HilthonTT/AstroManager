using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IEntityAttributeData
{
    Task CreateEntityAttributeAsync(EntityAttributeModel entity);
    Task DeleteEntityAttributeAsync(int id, int userId);
    Task<EntityAttributeModel> GetEntityAttributeByIdAsync(int id);
    Task<List<EntityAttributeModel>> GetEntityAttributesByUserIdAsync(int userId);
    Task UpdateEntityAttributeAsync(EntityAttributeModel entity);
}