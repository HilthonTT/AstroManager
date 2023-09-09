using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Enums;
using AstroManagerApi.Library.Extensions;
using AstroManagerApi.Library.Helpers;
using AstroManagerApi.Library.Models;
using Dapper;
using Microsoft.Extensions.Caching.Distributed;

namespace AstroManagerApi.Library.DataAccess;
public class AttributeData : IAttributeData
{
    private const string CacheName = nameof(AttributeData);
    private readonly ISqlDataAccess _sql;
    private readonly IDistributedCache _cache;

    public AttributeData(ISqlDataAccess sql, IDistributedCache cache)
    {
        _sql = sql;
        _cache = cache;
    }

    private static DynamicParameters GetCreateParameters(AttributeModel attribute)
    {
        var parameters = new DynamicParameters();
        parameters.Add("AttributeName", attribute.AttributeName);

        return parameters;
    }

    public async Task<List<AttributeModel>> GetAllAttributesAsync()
    {
        var output = await _cache.GetRecordAsync<List<AttributeModel>>(CacheName);
        if (output is null)
        {
            string storedProcedure = SqlHelper.GetStoredProcedure(DataType.Attribute, Operation.GetAll);

            output = await _sql.LoadDataAsync<AttributeModel>(storedProcedure);
            await _cache.SetRecordAsync(CacheName, output, TimeSpan.FromHours(10), TimeSpan.FromHours(5));
        }

        return output;
    }

    public async Task CreateAttributeAsync(AttributeModel attribute)
    {
        string storedProcedure = SqlHelper.GetStoredProcedure(DataType.Attribute, Operation.Create);
        var parameters = GetCreateParameters(attribute);

        await _sql.SaveDataAsync(storedProcedure, parameters);
    }
}
