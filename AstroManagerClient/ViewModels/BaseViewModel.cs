using AstroManagerClient.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    public static void OpenErrorPopup()
    {
        var message = new OpenErrorPopupMessage(true);
        WeakReferenceMessenger.Default.Send(message);
    }
}
