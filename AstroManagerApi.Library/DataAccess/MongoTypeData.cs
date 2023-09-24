using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Extensions.Interfaces;
using AstroManagerApi.Library.Models;
using MongoDB.Driver;

namespace AstroManagerApi.Library.DataAccess;
public class MongoTypeData : ITypeData
{
    private const string CacheName = nameof(MongoTypeData);
    private readonly IMongoCollection<TypeModel> _types;
    private readonly IDistributedCacheHelper _cache;

    public MongoTypeData(IDbConnection db, IDistributedCacheHelper cache)
    {
        _types = db.TypeCollection;
        _cache = cache;
    }

    public async Task<List<TypeModel>> GetAllTypesAsync()
    {
        var output = await _cache.GetRecordAsync<List<TypeModel>>(CacheName);
        if (output is null)
        {
            var results = await _types.FindAsync(_ => true);
            output = await results.ToListAsync();

            await _cache.SetRecordAsync(CacheName, output, TimeSpan.FromDays(1), TimeSpan.FromHours(12));
        }

        return output;
    }

    public async Task<TypeModel> CreateTypeAsync(TypeModel type)
    {
        await _types.InsertOneAsync(type);
        return type;
    }
}
