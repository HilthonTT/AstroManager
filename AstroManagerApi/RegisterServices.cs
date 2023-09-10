using AstroManagerApi.Library.DataAccess;
using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption;
using AstroManagerApi.Library.Encryption.Interfaces;

namespace AstroManagerApi;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddTransient<IAesEncryptor, AesEncryptor>();
        builder.Services.AddTransient<IRecoveryKeyGenerator, RecoveryKeyGenerator>();
        builder.Services.AddTransient<ITextHasher, TextHasher>();

        builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddTransient<IAttributeData, AttributeData>();
        builder.Services.AddTransient<IEntityAttributeData, EntityAttributeData>();
        builder.Services.AddTransient<IEntityData, EntityData>();
        builder.Services.AddTransient<IMasterPasswordData, MasterPasswordData>();
        builder.Services.AddTransient<IRecoveryKeyData, RecoveryKeyData>();
        builder.Services.AddTransient<IUserData, UserData>();
    }
}
