using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.MsalClient;
using AstroManagerClient.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotAbleToEnterMasterPassword))]
    [NotifyPropertyChangedFor(nameof(IsNotLoggedIn))]
    private bool _isLoggedIn;

    [ObservableProperty]
    private bool _isAbleToEnterMasterPassword;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotAbleToEnterMasterPassword))]
    private bool _hasMasterPassword;

    [ObservableProperty]
    private string _masterPassword;

    [ObservableProperty]
    private string _reEnteredMasterPassword;

    public bool IsNotLoggedIn => !IsLoggedIn;
    public bool IsNotAbleToEnterMasterPassword => IsLoggedIn && !HasMasterPassword;

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
        _api.AcquireHeaders(result.AccessToken);

        var verifiedUser = await UserVerifier.VerifyUserInformationAsync(_userEndpoint, result);

        _loggedInUser.Id = verifiedUser.Id;
        _loggedInUser.ObjectIdentifier = verifiedUser.ObjectIdentifier;
        _loggedInUser.DisplayName = verifiedUser.DisplayName;
        _loggedInUser.FirstName = verifiedUser.FirstName;
        _loggedInUser.LastName = verifiedUser.LastName;
        _loggedInUser.EmailAddress = verifiedUser.EmailAddress;

        await FetchMasterPasswordAsync(_loggedInUser.Id);

        IsLoggedIn = true;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        try
        {
            var result = await PCAWrapper.Instance.AcquireTokenSilentAsync(PCAWrapper._scopes);

            await VerifyUserDataAsync(result);
        }
        catch (MsalUiRequiredException)
        {
            var result = await PCAWrapper.Instance.AcquireTokenInteractiveAsync(PCAWrapper._scopes);

            await VerifyUserDataAsync(result);
        }
        catch (Exception ex)
        {
            IsLoggedIn = false;
            await ShowMessageAsync("Error logging you in.", ex.Message);
        }
    }

    [RelayCommand]
    private async Task VerifyMasterPasswordAsync()
    {
        try
        {
            if (await _passwordEndpoint.VerifyPasswordAsync(_loggedInUser.Id, MasterPassword))
            {
                var message = new UserLoggedInMessage(true);
                WeakReferenceMessenger.Default.Send(message);

                await LoadMainPageAsync();
            }
            else
            {
                await ShowMessageAsync("Incorrect", "Your password is incorrect.");
            }
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error verifying your password.", ex.Message);
        }
    }

    private async Task<bool> CanCreateMasterPassword()
    {
        if (string.IsNullOrWhiteSpace(_loggedInUser.Id))
        {
            await ShowMessageAsync("Not logged in.", "You must be logged in to create a master password.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(MasterPassword))
        {
            await ShowMessageAsync("Master password is empty.", "You must enter a master password.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(ReEnteredMasterPassword))
        {
            await ShowMessageAsync("Re-entered Master password is empty.", "You must enter your master password again.");
            return false;
        }

        if (MasterPassword.Equals(ReEnteredMasterPassword) is false)
        {
            await ShowMessageAsync("Wrong re-entered master password.", "You've inputed the wrong master password.");
            return false;
        }

        return true;
    }

    [RelayCommand]
    private async Task CreateMasterPasswordAsync()
    {
        if (await CanCreateMasterPassword() is false)
        {
            return;
        }

        try
        {
            var masterPassword = new MasterPasswordModel()
            {
                User = new BasicUserModel((UserModel)_loggedInUser),
                HashedPassword = MasterPassword,
            };

            await _passwordEndpoint.CreateMasterPasswordAsync(masterPassword);

            HasMasterPassword = true;
            IsAbleToEnterMasterPassword = true;
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Oops, something went wrong.", ex.Message);
        }
    }

    private async Task FetchMasterPasswordAsync(string id)
    {
        try
        {
            var masterPassword = await _passwordEndpoint.GetUsersMasterPasswordAsync(id);
            bool isMasterPassword = masterPassword is not null;

            HasMasterPassword = isMasterPassword;
            IsAbleToEnterMasterPassword = isMasterPassword;
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error fetching your master password.", ex.Message);
        }
    }
}
