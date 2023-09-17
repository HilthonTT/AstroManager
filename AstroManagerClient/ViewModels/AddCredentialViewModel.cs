using AstroManagerClient.Library.Models;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class AddCredentialViewModel : BaseViewModel
{
    [ObservableProperty]
    private CredentialDisplayModel _credential;

    [ObservableProperty]
    private TypeModel _type;

    [ObservableProperty]
    private string _imagePath = "noimage.png";

    [RelayCommand]
    private static void Close()
    {
        var message = new AddCredentialMessage(false);
        WeakReferenceMessenger.Default.Send(message);
    }
}
