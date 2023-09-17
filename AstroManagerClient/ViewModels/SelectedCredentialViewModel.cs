using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class SelectedCredentialViewModel : BaseViewModel
{
    public SelectedCredentialViewModel()
    {
        WeakReferenceMessenger.Default.Register<CredentialDisplayModel>(this, (r, m) =>
        {
            Credential = m;
        });
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCredential))]
    private CredentialDisplayModel _credential;

    public bool IsCredential => Credential is not null;


    [RelayCommand]
    private void CloseCredential()
    {
        Credential = null;
    }
}
