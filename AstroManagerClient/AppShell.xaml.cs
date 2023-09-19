using AstroManagerClient.Messages;
using AstroManagerClient.MsalClient;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        BindingContext = this;

        WeakReferenceMessenger.Default.Register<UserLoggedInMessage>(this, (r, m) =>
        {
            IsLoggedIn = m.Value;
        });
    }

    private string _selectedRoute;
    public string SelectedRoute
    {
        get { return _selectedRoute; }
        set
        {
            _selectedRoute = value;
            OnPropertyChanged();
        }
    }

    private bool _isLoggedIn;
    public bool IsLoggedIn
    {
        get { return _isLoggedIn; }
        set 
        { 
            _isLoggedIn = value; 
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsNotLoggedIn));
        }
    }

    public bool IsNotLoggedIn => !IsLoggedIn;

    public async void OnMenuItemChanged(object sender, CheckedChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SelectedRoute) is false)
        {
            await Current.GoToAsync($"//{SelectedRoute}");
        }
    }

    public async void OnLogoutImageTapped(object sender, EventArgs e)
    {
        await PCAWrapper.Instance.SignOutAsync();
        await Current.GoToAsync($"//login");

        IsLoggedIn = false;
    }
}
