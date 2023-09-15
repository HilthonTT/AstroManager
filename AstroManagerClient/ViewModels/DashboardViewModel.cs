using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AstroManagerClient.ViewModels;
public partial class DashboardViewModel : BaseViewModel
{
    private readonly ILoggedInUser _loggedInUser;
    private readonly ICredentialEndpoint _credentialEndpoint;

    public DashboardViewModel(ILoggedInUser loggedInUser, ICredentialEndpoint credentialEndpoint)
    {
        _loggedInUser = loggedInUser;
        _credentialEndpoint = credentialEndpoint;
    }

    [ObservableProperty]
    private ObservableCollection<CredentialModel> _credentials;

    [ObservableProperty]
    private ObservableCollection<string> _commonWords = new()
    {
        "password",
        "123456",
        "qwerty",
    };

    [ObservableProperty]
    private int _reusedPasswordCount;

    [ObservableProperty]
    private int _weakPasswordCount;

    [ObservableProperty]
    private int _twoFactorAuthPasswordCount;

    [RelayCommand]
    private async Task LoadCredentialsAsync()
    {
        var loadedCredentials = await _credentialEndpoint.GetUsersCredentialsAsync(_loggedInUser.Id);
        Credentials = new(loadedCredentials);

        GetReusedPassword();
        GetWeakPasswords();
        GetTwoFactorAuthPassword();
    }

    [RelayCommand]
    private async Task LoadHomePageWithFilteringAsync(string value)
    {
        var objectValue = value switch
        {
            "ReusedPasswords" => GetReusedPassword(),
            "WeakPasswords" => GetWeakPasswords(),
            "TwoFactor" => GetTwoFactorAuthPassword(),
            _ => new List<CredentialModel>()
        };

        var parameters = new Dictionary<string, object>()
        {
            { "Credentials", value },
        };

        await Shell.Current.GoToAsync(nameof(HomePage), true, parameters);
    }

    private List<CredentialModel> GetReusedPassword()
    {
        var passwordGroups = Credentials
        .Where(cred => cred.Fields.Any(field => field.Name == "Password"))
        .GroupBy(cred =>
            cred.Fields.First(field => field.Name == "Password").Value);

        ReusedPasswordCount = passwordGroups.Count(group => group.Count() > 1);

        var credentialsWithReusedPasswords = passwordGroups
            .Where(group => group.Count() > 1)
            .SelectMany(group => group)
            .ToList();

        return credentialsWithReusedPasswords.ToList();
    }

    private List<CredentialModel> GetWeakPasswords()
    {
        int minimumPasswordLength = 8;
        bool useDictionaryCheck = true;

        var weakPasswords = Credentials
            .Where(cred =>
            {
                var passwordField = cred.Fields.FirstOrDefault(
                    field => field.Name.Equals("Password", StringComparison.InvariantCultureIgnoreCase));

                if (passwordField is not null)
                {
                    var passwordValue = passwordField.Value;

                    return passwordValue.Length < minimumPasswordLength ||
                        (useDictionaryCheck && IsCommonWord(passwordValue)) ||
                        !HasComplexity(passwordValue);
                }
                
                return false;
            })
            .ToList();

        WeakPasswordCount = weakPasswords.Count;

        return weakPasswords;
    }

    private bool IsCommonWord(string password)
    {
        string lowercasePassword = password.ToLower();
        return CommonWords.Contains(lowercasePassword);
    }

    private static bool HasComplexity(string password)
    {
        bool hasUppercase = false;
        bool hasLowercase = false;
        bool hasNumber = false;
        bool hasSymbol = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c)) hasUppercase = true;
            if (char.IsLower(c)) hasLowercase = true;
            if (char.IsDigit(c)) hasNumber = true;
            if (char.IsSymbol(c) || char.IsPunctuation(c)) hasSymbol = true;
        }

        return hasUppercase && hasLowercase && hasNumber && hasSymbol;
    }

    private List<CredentialModel> GetTwoFactorAuthPassword()
    {
        string emailFieldName = "Email";

        var credentialsWithEmails = Credentials
            .Where(cred => cred.Fields.Any(field => field.Name == emailFieldName))
            .ToList();

        TwoFactorAuthPasswordCount = credentialsWithEmails.Count;

        return credentialsWithEmails;
    }
}
