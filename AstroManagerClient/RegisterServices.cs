using AstroManagerClient.Library.Api;
using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Library.Storage;
using AstroManagerClient.Library.Storage.Interfaces;
using AstroManagerClient.ViewModels;
using AstroManagerClient.Pages;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

#if WINDOWS
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endif

#if ANDROID
[assembly: Android.App.UsesPermission(Android.Manifest.Permission.Camera)]
#endif

namespace AstroManagerClient;
public static class RegisterServices
{
    private static void ModifyEntry()
    {
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoMoreBorders", (handler, view) =>
        {
#if WINDOWS
            handler.PlatformView.FontWeight = Microsoft.UI.Text.FontWeights.Thin;
#endif
        });
    }

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
                fonts.AddFont("fa_solid.ttf", "FontAwesome");
                fonts.AddFont("fabmdl2.ttf", "Fabric");
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

        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<DashboardPage>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));

        ModifyEntry();
    }
}
