using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Extensions;
using AstroManagerApi.Library.Models;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace AstroManagerApi.Library.DataAccess;
public class MongoTypeData : ITypeData
{
    private const string CacheName = nameof(MongoTypeData);
    private readonly IMongoCollection<TypeModel> _types;
    private readonly IDistributedCache _cache;

    public MongoTypeData(IDbConnection db, IDistributedCache cache)
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

    public async Task CreateTypeAsync(TypeModel type)
    {
        await _types.InsertOneAsync(type);
    }
}
