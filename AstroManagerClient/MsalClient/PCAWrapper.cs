namespace AstroManagerClient.MsalClient;
public class PCAWrapper
{
    public static PCAWrapper Instance { get; private set; } = new();

    private IPublicClientApplication PCA { get; }

    private bool UseEmbedded { get; set; } = false;

    private readonly string _tenantId;
    private readonly string _authority;
    private readonly string _clientId;
    public static string[] _scopes;

    private PCAWrapper()
    {
        var config = App.Services.GetService<IConfiguration>();

        _tenantId = config["AzureAd:TenantId"];
        _authority = config["AzureAd:Authority"];
        _clientId = config["AzureAd:ClientId"];
        _scopes = new string[] { config["AzureAd:Scopes"] };

        PCA = PublicClientApplicationBuilder
            .Create(_clientId)
            .WithRedirectUri(PlatformConfig.Instance.RedirectUri)
            .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
            .Build();
    }

    public async Task<AuthenticationResult> AcquireTokenSilentAsync(string[] scopes)
    {
        var accts = await PCA.GetAccountsAsync();
        var acct = accts.FirstOrDefault();

        var authResult = await PCA.AcquireTokenSilent(scopes, acct).ExecuteAsync();

        var tenantProfiles = acct.GetTenantProfiles();

        return authResult;
    }

    public async Task<AuthenticationResult> AcquireTokenInteractiveAsync(string[] scopes)
    {
        var systemWebViewOptions = new SystemWebViewOptions();
#if IOS
        // embedded view is not supported on Android
        if (UseEmbedded)
        {
            return await PCA.AcquireTokenInteractive(scopes)
                .WithUseEmbeddedWebView(true)
                .WithParentActivityOrWindow(PlatformConfig.Instance.ParentWindow)
                .ExecuteAsync();
        }

        // Hide the privacy prompt in iOS
        systemWebViewOptions.iOSHidePrivacyPrompt = true;
#endif
        return await PCA.AcquireTokenInteractive(scopes)
            .WithAuthority(_authority)
            .WithTenantId(_tenantId)
            .WithParentActivityOrWindow(PlatformConfig.Instance.ParentWindow)
            .WithUseEmbeddedWebView(true)
            .ExecuteAsync();
    }

    public static List<Claim> AcquireClaims(AuthenticationResult result)
    {
        var handler = new JwtSecurityTokenHandler();
        var idToken = handler.ReadJwtToken(result.IdToken);

        return idToken.Claims.ToList();
    }

    public async Task SignOutAsync()
    {
        SecureStorage.RemoveAll();
        var accounts = await PCA.GetAccountsAsync();
        foreach (var acct in accounts)
        {
            await PCA.RemoveAsync(acct);
        }

        var message = new UserLoggedInMessage(false);
        WeakReferenceMessenger.Default.Send(message);
    }
}
