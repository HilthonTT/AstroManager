using AstroManagerClient.ConstantsVariables;
using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Models;
using CommunityToolkit.Maui.Core.Extensions;
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
    [NotifyPropertyChangedFor(nameof(RecentlyUpdatedCredentials))]
    [NotifyPropertyChangedFor(nameof(ReusedCredentials))]
    [NotifyPropertyChangedFor(nameof(WeakCredentials))]
    [NotifyPropertyChangedFor(nameof(TwoFactorCredentials))]
    private ObservableCollection<CredentialDisplayModel> _credentials = new();

    public ObservableCollection<CredentialDisplayModel> RecentlyUpdatedCredentials => Credentials
        .OrderByDescending(x => x.DateModified).Take(3).ToObservableCollection();

    public ObservableCollection<CredentialDisplayModel> ReusedCredentials => GetReusedPassword();
    public ObservableCollection<CredentialDisplayModel> WeakCredentials => GetWeakPasswords();
    public ObservableCollection<CredentialDisplayModel> TwoFactorCredentials => GetTwoFactorAuth();


    [RelayCommand]
    private async Task LoadCredentialsAsync()
    {
        var credentials = await _credentialEndpoint.GetUsersCredentialsAsync(_loggedInUser.Id);
        var mappedCredentials = credentials.Select(x => new CredentialDisplayModel(x)).ToList();

        Credentials = new(mappedCredentials);
    }

    private static bool IsCommonWord(string password)
    {
        string lowercasePassword = password.ToLower();
        return Constants.CommonWords.Contains(lowercasePassword);
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

    private ObservableCollection<CredentialDisplayModel> GetReusedPassword()
    {
        var passwordGroups = Credentials
            .Where(cred => cred.Fields != null)
            .Where(cred => cred.Fields.Any(field => string.Equals(
                field.Name, "Password", StringComparison.InvariantCultureIgnoreCase)))
            .GroupBy(cred =>
        cred.Fields.First(field => string.Equals(
            field.Name, "Password", StringComparison.InvariantCultureIgnoreCase)).Value);

        var reusedPasswords = passwordGroups.Count(group => group.Count() > 1);

        var credentialsWithReusedPasswords = passwordGroups
            .Where(group => group.Count() > 1)
            .SelectMany(group => group)
            .ToList();

        return credentialsWithReusedPasswords.ToObservableCollection();
    }

    private ObservableCollection<CredentialDisplayModel> GetWeakPasswords()
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
            .ToObservableCollection();

        return weakPasswords;
    }

    private ObservableCollection<CredentialDisplayModel> GetTwoFactorAuth()
    {
        string emailFieldName = "Email";

        var credentialsWithEmails = Credentials
            .Where(cred => cred.Fields.Any(field => string.Equals(
                field.Name, emailFieldName, StringComparison.InvariantCultureIgnoreCase)))
            .ToObservableCollection();

        return credentialsWithEmails;
    }
}
