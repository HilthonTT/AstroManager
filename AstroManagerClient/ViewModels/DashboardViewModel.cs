using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using AstroManagerClient.Models.Interfaces;
using AstroManagerClient.Pages;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace AstroManagerClient.ViewModels;
public partial class DashboardViewModel : BaseViewModel
{
    private readonly ILoggedInUser _loggedInUser;
    private readonly ICredentialEndpoint _credentialEndpoint;
    private readonly IWeakPasswordEndpoint _weakPasswordEndpoint;
    private readonly IErrorDisplayModel _error;

    public DashboardViewModel(
        ILoggedInUser loggedInUser,
        ICredentialEndpoint credentialEndpoint,
        IWeakPasswordEndpoint weakPasswordEndpoint,
        IErrorDisplayModel error)
    {
        _loggedInUser = loggedInUser;
        _credentialEndpoint = credentialEndpoint;
        _weakPasswordEndpoint = weakPasswordEndpoint;
        _error = error;
        WeakReferenceMessenger.Default.Register<CloseFilterPopupMessage>(this, async (r, m) =>
        {
            await LoadCredentialsAsync();

            if (m.Value.Name.Equals("All", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var filteredCredentials = Credentials.Where(x => x.Type.Name.Contains(m.Value.Name)).ToList();
            FilterableCredentials = new(filteredCredentials);
        });
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RecentlyUpdatedCredentials))]
    [NotifyPropertyChangedFor(nameof(ReusedCredentials))]
    [NotifyPropertyChangedFor(nameof(WeakCredentials))]
    [NotifyPropertyChangedFor(nameof(TwoFactorCredentials))]
    private ObservableCollection<CredentialDisplayModel> _credentials = new();

    [ObservableProperty]
    private ObservableCollection<string> _commonWords = new();

    [ObservableProperty]
    private ObservableCollection<CredentialDisplayModel> _filterableCredentials;

    public ObservableCollection<CredentialDisplayModel> RecentlyUpdatedCredentials => Credentials
        .OrderByDescending(x => x.DateModified).Take(3).ToObservableCollection();

    public ObservableCollection<CredentialDisplayModel> ReusedCredentials => GetReusedPassword();
    public ObservableCollection<CredentialDisplayModel> WeakCredentials => GetWeakPasswords();
    public ObservableCollection<CredentialDisplayModel> TwoFactorCredentials => GetTwoFactorAuth();


    [RelayCommand]
    private async Task LoadCredentialsAsync()
    {
        try
        {
            var commonWords = await _weakPasswordEndpoint.GetWeakPasswordAsync();
            CommonWords = new(commonWords);

            var credentials = await _credentialEndpoint.GetUsersCredentialsAsync(_loggedInUser.Id);
            var mappedCredentials = credentials.Select(x => new CredentialDisplayModel(x)).ToList();

            Credentials = new(mappedCredentials);
            FilterableCredentials = new(mappedCredentials);
        }
        catch (Exception ex)
        {
            _error.SetErrorMessage(ex.Message);
            OpenErrorPopup();
        } 
    }

    [RelayCommand]
    private async Task OpenHomePageAsync()
    {
        await Shell.Current.GoToAsync(nameof(HomePage), true);
    }

    [RelayCommand]
    private static void OpenFilterPopup()
    {
        var message = new OpenFilterPopupMessage(true);
        WeakReferenceMessenger.Default.Send(message);
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
                        (useDictionaryCheck && CommonWords.Contains(passwordValue)) ||
                        !HasComplexity(passwordValue);
                }

                return false;
            })
            .ToObservableCollection();

        return weakPasswords;
    }

    private static bool IsEmailAddress(string text)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(text, pattern);
    }

    private ObservableCollection<CredentialDisplayModel> GetTwoFactorAuth()
    {
        string userNameField = "Username";

        var credentialsWithEmails = Credentials
            .Where(cred => cred.Fields.Any(field => field.Name.Equals(
                userNameField, StringComparison.InvariantCultureIgnoreCase) &&
                IsEmailAddress(field.Value))) 
            .ToObservableCollection();

        return credentialsWithEmails;
    }
}
