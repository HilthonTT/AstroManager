namespace AstroManagerClient;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    public App(IServiceProvider services)
    {
        Services = services;

        InitializeComponent();

        MainPage = new AppShell();
    }
}
