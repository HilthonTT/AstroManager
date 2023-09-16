using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Models;
using AstroManagerClient.Pages;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AstroManagerClient.ViewModels;
public partial class HomeViewModel : BaseViewModel, IQueryAttributable
{
    private readonly ILoggedInUser _loggedInUser;
    private readonly ICredentialEndpoint _credentialEndpoint;
    private readonly ITypeEndpoint _typeEndpoint;

    public const string Title = "Title";
    public const string DateCreated = "DateCreated";
    public const string DateModified = "DateModified";

    public HomeViewModel(
        ILoggedInUser loggedInUser,
        ICredentialEndpoint credentialEndpoint,
        ITypeEndpoint typeEndpoint)
    {
        _loggedInUser = loggedInUser;
        _credentialEndpoint = credentialEndpoint;
        _typeEndpoint = typeEndpoint;
    }

    [ObservableProperty]
    private ObservableCollection<CredentialDisplayModel> _credentials;

    [ObservableProperty]
    private ObservableCollection<TypeModel> _types;

    [ObservableProperty]
    private CredentialDisplayModel _selectedCredential;

    [ObservableProperty]
    private TypeModel _selectedType;

    [ObservableProperty]
    private string _filtering;

    [RelayCommand]
    private async Task LoadCredentialsAsync()
    {
        var loadedCredentials = await _credentialEndpoint.GetUsersCredentialsAsync(_loggedInUser.Id);
        Credentials = new(loadedCredentials.Select(x => new CredentialDisplayModel(x)));
    }

    [RelayCommand]
    private async Task LoadTypesAsync()
    {
        var loadedTypes = await _typeEndpoint.GetAllTypesAsync();
        Types = new(loadedTypes);
    }

    [RelayCommand]
    private void OnFilteringClick(string filtering)
    {
        Filtering = filtering;
    }

    [RelayCommand]
    private void OnCredentialClick(CredentialDisplayModel credential)
    {
        SelectedCredential = credential;
    }

    [RelayCommand]
    private async Task PreferencesAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(SettingsPage)}?sub=appearance");
    }

    [RelayCommand]
    private async Task DeleteCredentialAsync()
    {
        if (SelectedCredential is null)
        {
            return;
        }

        IsBusy = true;
        try
        {
            Credentials.Remove(Credentials.FirstOrDefault(x => x.Id == SelectedCredential.Id));

            var credentialToDelete = new CredentialModel()
            {
                Id = SelectedCredential.Id,
            };

            SelectedCredential = null;
            await _credentialEndpoint.DeleteCredentialAsync(credentialToDelete);
        }
        catch (Exception)
        {
            // TODO: Move to error page
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SearchCredentialsAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query) is false)
        {
            await LoadCredentialsAsync();
            Credentials = Credentials.OrderByDescending(x => x.Title.Contains(query)).ToObservableCollection();
        }
    }

    [RelayCommand]
    private async Task FilterCredentialsAsync()
    {
        await LoadCredentialsAsync();

        switch (Filtering)
        {
            case DateCreated:
                Credentials = SortCredentialsByDate(Credentials, x => x.DateAdded, x => x.DateModified);
                break;

            case DateModified:
                Credentials = SortCredentialsByDate(Credentials, x => x.DateModified, x => x.DateAdded);
                break;

            default:
                break;
        }
    }

    private ObservableCollection<CredentialDisplayModel> SortCredentialsByDate(
        ObservableCollection<CredentialDisplayModel> credentials,
        Func<CredentialDisplayModel, DateTime> primarySortKey,
        Func<CredentialDisplayModel, DateTime> secondarySortKey)
    {
        return credentials
            .OrderByDescending(primarySortKey)
            .ThenByDescending(secondarySortKey)
            .ToObservableCollection();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var credentials = query["Credentials"] as List<CredentialDisplayModel>;
        Credentials = new(credentials);
    }
}
