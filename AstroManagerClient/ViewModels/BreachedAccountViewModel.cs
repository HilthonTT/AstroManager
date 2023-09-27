using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace AstroManagerClient.ViewModels;
public partial class BreachedAccountViewModel : BaseViewModel
{
    private readonly IPasswordBreacherEndpoint _breacherEndpoint;
    private readonly ILoggedInUser _loggedInUser;
    private readonly ICredentialEndpoint _credentialEndpoint;

    public BreachedAccountViewModel(
        IPasswordBreacherEndpoint breacherEndpoint,
        ILoggedInUser loggedInUser,
        ICredentialEndpoint credentialEndpoint)
    {
        _breacherEndpoint = breacherEndpoint;
        _loggedInUser = loggedInUser;
        _credentialEndpoint = credentialEndpoint;
    }

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private ObservableCollection<CredentialDisplayModel> _credentials;

    [RelayCommand]
    private async Task LoadBreachedCredentialsAsync()
    {
        IsLoading = true;

        try
        {
            var credentials = await _credentialEndpoint.GetUsersCredentialsAsync(_loggedInUser.Id);
            var breachedCredentials = await _breacherEndpoint.GetBreachedCredentialsAsync(credentials);

            var mappedCredentials = breachedCredentials.Select(x => new CredentialDisplayModel(x));

            Credentials = new(mappedCredentials);
        }
        catch (Exception ex)
        {
            // Implement error message popup
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private static void CredentialClick(CredentialDisplayModel credential)
    {
        var message = new ViewCredentialMessage(credential);
        WeakReferenceMessenger.Default.Send(message);
    }
}
