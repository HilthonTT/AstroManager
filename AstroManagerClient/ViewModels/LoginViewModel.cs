using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.MsalClient;
using AstroManagerClient.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Identity.Client;

namespace AstroManagerClient.ViewModels;
public partial class LoginViewModel : BaseViewModel
{
    private readonly IApiHelper _api;
    private readonly ILoggedInUser _loggedInUser;
    private readonly IUserEndpoint _userEndpoint;
    private readonly IMasterPasswordEndpoint _passwordEndpoint;

    public LoginViewModel(
        IApiHelper api,
        ILoggedInUser loggedInUser,
        IUserEndpoint userEndpoint,
        IMasterPasswordEndpoint passwordEndpoint)
    {
        _api = api;
        _loggedInUser = loggedInUser;
        _userEndpoint = userEndpoint;
        _passwordEndpoint = passwordEndpoint;

        SecureStorage.RemoveAll();
    }

    [ObservableProperty]
    private bool _isLoggedIn;

    [ObservableProperty]
    private string _masterPassword;

    private async Task LoadMainPageAsync()
    {
        await Shell.Current.GoToAsync(nameof(HomePage), true);
    }

    private static async Task ShowMessageAsync(string title, string message)
    {
        await Shell.Current.DisplayAlert(title, message, "OK");
    }

    private async Task VerifyUserDataAsync(AuthenticationResult result)
    {
        IsLoggedIn = true;
        _api.AcquireHeaders(result.AccessToken);

        var verifiedUser = await UserVerifier.VerifyUserInformationAsync(_userEndpoint, result);

        _loggedInUser.Id = verifiedUser.Id;
        _loggedInUser.ObjectIdentifier = verifiedUser.ObjectIdentifier;
        _loggedInUser.DisplayName = verifiedUser.DisplayName;
        _loggedInUser.FirstName = verifiedUser.FirstName;
        _loggedInUser.LastName = verifiedUser.LastName;
        _loggedInUser.EmailAddress = verifiedUser.EmailAddress;

        await Shell.Current.GoToAsync(nameof(HomePage), true);
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            var result = await PCAWrapper.Instance.AcquireTokenSilentAsync(PCAWrapper.Scopes);
            IsLoggedIn = true;

            await VerifyUserDataAsync(result);
        }
        catch (MsalUiRequiredException)
        {
            var result = await PCAWrapper.Instance.AcquireTokenInteractiveAsync(PCAWrapper.Scopes);

            IsLoggedIn = true;

            await VerifyUserDataAsync(result);
        }
        catch (Exception ex) 
        {
            IsLoggedIn = false;
            await ShowMessageAsync("Error logging you in.", ex.Message);
        }
    }

    [RelayCommand]
    private async Task VerifyMasterPassword()
    {
        try
        {
            bool isCorrect = await _passwordEndpoint.VerifyPasswordAsync(_loggedInUser.Id, MasterPassword);

            if (isCorrect)
            {
                await LoadMainPageAsync();
            }
            else
            {
                await ShowMessageAsync("Incorrect", "Your password is incorrect.");
            }
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error verifying your password", ex.Message);
        }
    }
}
