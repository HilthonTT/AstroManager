using AstroManagerClient.Library.Api.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace AstroManagerClient.ViewModels;
public partial class CredentialViewModel : BaseViewModel
{
    private readonly ICredentialEndpoint _credentialEndpoint;

    public CredentialViewModel(ICredentialEndpoint credentialEndpoint)
    {
        _credentialEndpoint = credentialEndpoint;
    }

    [RelayCommand]
    private async Task UpdateCredentialAsync()
    {

    }
}
