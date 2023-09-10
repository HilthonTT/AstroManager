namespace AstroManagerClient.MsalClient;
public class PlatformConfig
{
    public static PlatformConfig Instance { get; } = new();
    public string RedirectUri { get; set; } = "msalauthinmaui://auth";
    public object ParentWindow { get; set; }

    private PlatformConfig()
    {
        
    }
}
