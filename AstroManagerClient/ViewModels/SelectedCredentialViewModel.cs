using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AstroManagerClient.ViewModels;
public partial class SelectedCredentialViewModel : BaseViewModel
{
    [ObservableProperty]
    private CredentialDisplayModel _credential;

    [RelayCommand]
    private async Task UpdateCredentialAsync()
    {

    }
}
