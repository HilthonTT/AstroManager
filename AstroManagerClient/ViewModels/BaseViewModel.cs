namespace AstroManagerClient.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    public LocalizationResourceManager LocalizationResourceManager => LocalizationResourceManager.Instance;

    public static void OpenErrorPopup()
    {
        var message = new OpenErrorPopupMessage(true);
        WeakReferenceMessenger.Default.Send(message);
    }
}
