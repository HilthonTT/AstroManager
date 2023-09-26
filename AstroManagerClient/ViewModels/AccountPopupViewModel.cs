using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class AccountPopupViewModel : BaseViewModel
{
    private readonly IMasterPasswordEndpoint _passwordEndpoint;
    private readonly ILoggedInUser _loggedInUser;

    public AccountPopupViewModel()
    {
        _passwordEndpoint = App.Services.GetService<IMasterPasswordEndpoint>();
        _loggedInUser = App.Services.GetService<ILoggedInUser>();

        User = (UserModel)_loggedInUser;
    }

    [ObservableProperty]
    private string _errorMessage;

    [ObservableProperty]
    private string _masterPassword;

    [ObservableProperty]
    private string _newPassword;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsVerifyPasswordVisible))]
    private bool _isCorrectPassword = false;

    [ObservableProperty]
    private bool _incorrectPassword = false;

    [ObservableProperty]
    private UserModel _user;

    public bool IsVerifyPasswordVisible => !IsCorrectPassword;

    private static void ClosePopup()
    {
        var message = new OpenAccountPopupMessage(false);
        WeakReferenceMessenger.Default.Send(message);
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

            ClosePopup();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Something went wrong on our side. {ex.Message}";
        }
    }
}
