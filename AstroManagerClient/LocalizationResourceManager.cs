using AstroManagerClient.Resources.Languages;
using System.ComponentModel;
using System.Globalization;

namespace AstroManagerClient;
public class LocalizationResourceManager : INotifyPropertyChanged
{
    private LocalizationResourceManager()
    {
        AppResources.Culture = CultureInfo.CurrentCulture;
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
                PropertyChanged?.Invoke(nameof(Today), new PropertyChangedEventArgs(null));
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
        Today = DateTime.Now.ToString("dddd, MMMM dd yyyy", AppResources.Culture);


        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }
}
