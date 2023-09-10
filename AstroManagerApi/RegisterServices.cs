using AstroManagerApi.Library.DataAccess;
using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption;
using AstroManagerApi.Library.Encryption.Interfaces;
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

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
            options.InstanceName = "AstroManager_";
        });

        builder.Services.AddTransient<IAesEncryptor, AesEncryptor>();
        builder.Services.AddTransient<IRecoveryKeyGenerator, RecoveryKeyGenerator>();
        builder.Services.AddTransient<ITextHasher, TextHasher>();
    }
}
