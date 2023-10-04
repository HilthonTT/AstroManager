using AstroManagerApi.Data;
using AstroManagerApi.Data.Interfaces;
using AstroManagerApi.Library.DataAccess;
using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Extensions;
using AstroManagerApi.Library.Extensions.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace AstroManagerApi;

public static class RegisterServices
{
    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddInMemoryTokenCaches();

        builder.Services.AddAuthorization();
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddMemoryCache();

        builder.Services.AddHttpClient("AstroManagerClient", client =>
        {
            client.DefaultRequestHeaders.Add("User-Agent", "AstroManager");
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
            options.InstanceName = "AstroManager_";
        });
        
        builder.Services.AddTransient<IDistributedCacheHelper, DistributedCacheHelper>();
        builder.Services.AddTransient<IRecoveryKeyGenerator, RecoveryKeyGenerator>();
        builder.Services.AddTransient<ITextHasher, TextHasher>();

        builder.Services.AddSingleton<IAesEncryptor, AesEncryptor>();
        builder.Services.AddSingleton<IDbConnection, DbConnection>();
        builder.Services.AddSingleton<ICredentialData, MongoCredentialData>();
        builder.Services.AddSingleton<ICredentialTemplateData, MongoCredentialTemplateData>();
        builder.Services.AddSingleton<IMasterPasswordData, MongoMasterPasswordData>();
        builder.Services.AddSingleton<IRecoveryKeyData, MongoRecoveryKeyData>();
        builder.Services.AddSingleton<ITypeData, MongoTypeData>();
        builder.Services.AddSingleton<IUserData, MongoUserData>();

        builder.Services.AddTransient<IDummyDataService, DummyDataService>();
    }
}
