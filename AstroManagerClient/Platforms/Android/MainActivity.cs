using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AstroManagerClient.MsalClient;
using Microsoft.Identity.Client;

namespace AstroManagerClient;
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private const string AndroidRedirectURI = $"msauth://com.companyname.msalauthinmaui/snaHlgr4autPsfVDSBVaLpQXnqU=";

    protected override void OnCreate(Bundle savedInstanceState)
    {
        PlatformConfig.Instance.RedirectUri = AndroidRedirectURI;
        PlatformConfig.Instance.ParentWindow = this;
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
    }
}
