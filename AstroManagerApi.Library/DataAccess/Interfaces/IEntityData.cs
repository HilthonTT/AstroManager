using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IEntityData
{
    Task CreateEntityAsync(EntityModel entity);
    Task<List<EntityModel>> GetAllEntitiesAsync();
}