using AstroManagerClient.Library.Models;
using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AstroManagerClient.ViewModels;
public partial class AddCredentialViewModel : BaseViewModel
{
    [ObservableProperty]
    private CredentialDisplayModel _credential;

    [ObservableProperty]
    private TypeModel _type;

    [ObservableProperty]
    private string _imagePath = "noimage.png";
}
