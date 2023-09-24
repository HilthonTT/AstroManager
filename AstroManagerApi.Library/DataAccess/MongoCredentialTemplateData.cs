using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Extensions.Interfaces;
using AstroManagerApi.Library.Models;
using MongoDB.Driver;

namespace AstroManagerApi.Library.DataAccess;
public class MongoCredentialTemplateData : ICredentialTemplateData
{
    private const string CacheName = nameof(MongoCredentialTemplateData);
    private const string CacheNamePrefix = $"{CacheName}_";
    private readonly IMongoCollection<CredentialTemplateModel> _templates;
    private readonly IDistributedCacheHelper _cache;

    public MongoCredentialTemplateData(
        IDbConnection db,
        IDistributedCacheHelper cache)
    {
        _templates = db.CredentialTemplateCollection;
        _cache = cache;
    }

    public async Task<List<CredentialTemplateModel>> GetAllTemplatesAsync()
    {
        var output = await _cache.GetRecordAsync<List<CredentialTemplateModel>>(CacheName);
        if (output is null)
        {
            var results = await _templates.FindAsync(_ => true);
            output = await results.ToListAsync();

            await _cache.SetRecordAsync(CacheName, output, TimeSpan.FromHours(5), TimeSpan.FromHours(3));
        }

        return output;
    }

    public async Task<CredentialTemplateModel> GetTemplateAsync(string id)
    {
        string key = CacheNamePrefix + id;

        var output = await _cache.GetRecordAsync<CredentialTemplateModel>(key);
        if (output is null)
        {
            var results = await _templates.FindAsync(_ => true);
            output = await results.FirstOrDefaultAsync();

            await _cache.SetRecordAsync(CacheName, output, TimeSpan.FromHours(5), TimeSpan.FromHours(3));
        }

        return output;
    }

    public async Task<CredentialTemplateModel> CreateTemplateAsync(CredentialTemplateModel template)
    {
        await _templates.InsertOneAsync(template);
        return template;
    }
}
