namespace AstroManagerClient;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        BindingContext = this;
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

    async void OnMenuItemChanged(object sender, CheckedChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_selectedRoute) is false)
        {
            await Current.GoToAsync($"//{_selectedRoute}");
        }
    }
}
