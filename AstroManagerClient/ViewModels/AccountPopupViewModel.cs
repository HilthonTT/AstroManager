using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AstroManagerClient.ViewModels;
public partial class AccountPopupViewModel : BaseViewModel
{
    private readonly IMasterPasswordEndpoint _passwordEndpoint;
    private readonly ILoggedInUser _loggedInUser;

    public AccountPopupViewModel()
    {
        _passwordEndpoint = App.Services.GetService<IMasterPasswordEndpoint>();
        _loggedInUser = App.Services.GetService<ILoggedInUser>();
    }

    [ObservableProperty]
    private string _errorMessage;

    [ObservableProperty]
    private string _masterPassword;

    [ObservableProperty]
    private string _newPassword;

    [ObservableProperty]
    private bool _isCorrectPassword = false;

    [ObservableProperty]
    private bool _incorrectPassword = false;

    [RelayCommand]
    private void CloseErrorMessage()
    {
        ErrorMessage = "";
    }

    [RelayCommand]
    private async Task VerifyPasswordAsync()
    {
        ErrorMessage = "";

        try
        {
            bool isCorrect = await _passwordEndpoint.VerifyPasswordAsync(_loggedInUser.Id, MasterPassword);
            if (isCorrect is false)
            {
                IncorrectPassword = true;
                ErrorMessage = "Incorrect master password";
                return;
            }

            IsCorrectPassword = true;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Something went wrong on our side. {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task ChangePasswordAsync()
    {
        ErrorMessage = "";

        if (IsCorrectPassword is false)
        {
            ErrorMessage = "Incorrect Password.";
            return;
        }

        try
        {
            MasterPassword = NewPassword;

            var userMasterPassword = await _passwordEndpoint.GetUsersMasterPasswordAsync(_loggedInUser.Id);
            userMasterPassword.HashedPassword = NewPassword;

            var passwordResetRequest = new PasswordResetModel
            {
                Master = new(userMasterPassword)
            };

            await _passwordEndpoint.UpdateMasterPasswordAsync(passwordResetRequest);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Something went wrong on our side. {ex.Message}";
        }
    }
}
