using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class SelectedCredentialViewModel : BaseViewModel
{
    [ObservableProperty]
    private CredentialDisplayModel _credential;

    public SelectedCredentialViewModel()
    {
        WeakReferenceMessenger.Default.Register<CredentialDisplayModel>(this, (r, m) =>
        {
            Credential = m;
        });
    }
}
