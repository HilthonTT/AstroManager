namespace AstroManagerClient;
public class LocalizationResourceManager : INotifyPropertyChanged
{
    private LocalizationResourceManager()
    {
        AppResources.Culture = CultureInfo.CurrentCulture;
        CurrentCulture = CultureInfo.CurrentCulture;
        Today = DateTime.Now.ToString("dddd, MMMM dd yyyy", AppResources.Culture);
    }

    private string _today;

    public string Today
    {
        get { return _today; }
        set 
        {
            _today = value; 
            if (_today != value)
            {
                PropertyChanged?.Invoke(nameof(Today), new(null));
            }
        }
    }

    private CultureInfo _currentCulture;

    public CultureInfo CurrentCulture
    {
        get { return _currentCulture; }
        set 
        { 
            _currentCulture = value; 
            if (_currentCulture != value)
            {
                PropertyChanged?.Invoke(nameof(CurrentCulture), new(null));
            }
        }
    }


    public static LocalizationResourceManager Instance { get; } = new();

    public object this[string resourceKey]
        => AppResources.ResourceManager.GetObject(resourceKey, AppResources.Culture) ?? Array.Empty<byte>();

    public event PropertyChangedEventHandler PropertyChanged;

    public void SetCulture(CultureInfo culture) 
    {
        AppResources.Culture = culture;

        CurrentCulture = culture;
        Today = DateTime.Now.ToString("dddd, MMMM dd yyyy", AppResources.Culture);

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }
}
