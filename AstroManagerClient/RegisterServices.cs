using AstroManagerClient.Library.Api;
using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Library.Storage;
using AstroManagerClient.Library.Storage.Interfaces;
using AstroManagerClient.ViewModels;
using AstroManagerClient.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AstroManagerClient;
public static class RegisterServices
{
    private static IConfiguration AddConfiguration()
    {
        string configFilePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var builder = new ConfigurationBuilder()
            .AddJsonFile(configFilePath, optional: false, reloadOnChange: true);

        return builder.Build();
    }

    public static void ConfigureServices(this MauiAppBuilder builder)
    {
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddHttpClient();
        builder.Configuration.AddConfiguration(AddConfiguration());

        builder.Services.AddSingleton<ILoggedInUser, UserModel>();
        builder.Services.AddSingleton<IApiHelper, ApiHelper>();

        builder.Services.AddTransient<ISecureStorageWrapper, SecureStorageWrapper>();
        builder.Services.AddTransient<ICredentialEndpoint, CredentialEndpoint>();
        builder.Services.AddTransient<ICredentialTemplateEndpoint, CredentialTemplateEndpoint>();
        builder.Services.AddTransient<IMasterPasswordEndpoint, MasterPasswordEndpoint>();
        builder.Services.AddTransient<IRecoveryKeyEndpoint, RecoveryKeyEndpoint>();
        builder.Services.AddTransient<ITypeEndpoint, TypeEndpoint>();
        builder.Services.AddTransient<IUserEndpoint, UserEndpoint>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<LoginViewModel>();

        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
    }
}
