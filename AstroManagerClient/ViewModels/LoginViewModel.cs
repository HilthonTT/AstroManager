namespace AstroManagerClient.ViewModels;
public partial class LoginViewModel : BaseViewModel
{
    private readonly IApiHelper _api;
    private readonly ILoggedInUser _loggedInUser;
    private readonly IMasterPasswordEndpoint _passwordEndpoint;
    private readonly IErrorDisplayModel _error;

    public LoginViewModel(
        IApiHelper api,
        ILoggedInUser loggedInUser,
        IMasterPasswordEndpoint passwordEndpoint,
        IErrorDisplayModel error)
    {
        _api = api;
        _loggedInUser = loggedInUser;
        _passwordEndpoint = passwordEndpoint;
        _error = error;
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

    private void ShowMessage(string message)
    {
        _error.SetErrorMessage(message);
        OpenErrorPopup();
    }

    private async Task VerifyUserDataAsync(AuthenticationResult result)
    {
        _api.AcquireHeaders(result.AccessToken);

        var verifiedUser = await UserVerifier.VerifyUserInformationAsync(result);
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
            ShowMessage(ex.Message);
        }
    }

    [RelayCommand]
    private async Task VerifyMasterPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(MasterPassword))
        {
            ShowMessage(LocalizationResourceManager["MustEnterPassword"].ToString());
            return;
        }

        try
        {
            bool isPasswordCorrect = await _passwordEndpoint.VerifyPasswordAsync(_loggedInUser.Id, MasterPassword);
            if (isPasswordCorrect is false)
            {
                ShowMessage(LocalizationResourceManager["PasswordIncorrect"].ToString());
                return;
            }

            var message = new UserLoggedInMessage(true);
            WeakReferenceMessenger.Default.Send(message);

            await LoadMainPageAsync();
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
        }
        finally
        {
            MasterPassword = "";
        }
    }

    private bool CanCreateMasterPassword()
    {
        if (string.IsNullOrWhiteSpace(_loggedInUser.Id))
        {
            ShowMessage(LocalizationResourceManager["MustBeLoggedIn"].ToString());
            return false;
        }

        if (string.IsNullOrWhiteSpace(MasterPassword))
        {
            ShowMessage(LocalizationResourceManager["MustEnterPassword"].ToString());
            return false;
        }

        if (string.IsNullOrWhiteSpace(ReEnteredMasterPassword))
        {
            ShowMessage(LocalizationResourceManager["MustReEnterPassword"].ToString());
            return false;
        }

        if (MasterPassword.Equals(ReEnteredMasterPassword) is false)
        {
            ShowMessage(LocalizationResourceManager["PasswordDontMatch"].ToString());
            return false;
        }

        return true;
    }

    [RelayCommand]
    private async Task CreateMasterPasswordAsync()
    {
        if (CanCreateMasterPassword() is false)
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
            MasterPassword = "";
        }
        catch (Exception ex)
        {
            ShowMessage(ex.Message);
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
            ShowMessage(ex.Message);
        }
    }
}
