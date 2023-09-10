using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AstroManagerClient.MsalClient;
public class PCAWrapper
{
    public static PCAWrapper Instance { get; private set; } = new();

    private IPublicClientApplication PCA { get; }

    private bool UseEmbedded { get; set; } = false;

    private const string TenantId = "";
    private const string Authority = $"https://login.microsoftonline.com/{TenantId}";
    private const string ClientId = "";
    public static string[] Scopes = { $"api://{ClientId}/access_as_user" };

    private PCAWrapper()
    {
        PCA = PublicClientApplicationBuilder
            .Create(ClientId)
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
        if (UseEmbedded)
	    {
            return await PCA
                .AcquireTokenInteractive(scopes)
                .WithUseEmbeddedWebView(true)
                .WithParentActivityOrWindow(PlatformConfig.Instance.ParentWindow)
                .ExecuteAsync();
	    }

        systemWebViewOptions.iOSHidePrivacyPrompt = true;
#endif

        return await PCA.AcquireTokenInteractive(scopes)
            .WithAuthority(Authority)
            .WithTenantId(TenantId)
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
        var accounts = await PCA.GetAccountsAsync();
        foreach (var acct in accounts)
        {
            await PCA.RemoveAsync(acct);
        }
    }
}
