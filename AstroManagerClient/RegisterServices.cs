using AstroManagerClient.Library.Api;
using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using Microsoft.Extensions.Logging;

namespace AstroManagerClient;
public static class RegisterServices
{
    public static void ConfigureServices(this MauiAppBuilder builder)
    {
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<ILoggedInUser, UserModel>();
        builder.Services.AddSingleton<IApiHelper, ApiHelper>();

        builder.Services.AddTransient<ICredentialEndpoint, CredentialEndpoint>();
        builder.Services.AddTransient<ICredentialTemplateEndpoint, CredentialTemplateEndpoint>();
        builder.Services.AddTransient<IMasterPasswordEndpoint, MasterPasswordEndpoint>();
        builder.Services.AddTransient<IRecoveryKeyEndpoint, RecoveryKeyEndpoint>();
        builder.Services.AddTransient<ITypeEndpoint, TypeEndpoint>();
        builder.Services.AddTransient<IUserEndpoint, UserEndpoint>();
    }
}
