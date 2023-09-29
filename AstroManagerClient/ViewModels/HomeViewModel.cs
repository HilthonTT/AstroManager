using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using AstroManagerClient.Pages;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace AstroManagerClient.ViewModels;
public partial class HomeViewModel : BaseViewModel
{
    private readonly ILoggedInUser _loggedInUser;
    private readonly ICredentialEndpoint _credentialEndpoint;
    private readonly ITypeEndpoint _typeEndpoint;

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
    [NotifyPropertyChangedFor(nameof(TypeNames))]
    private ObservableCollection<TypeModel> _types;

    [ObservableProperty]
    private CredentialDisplayModel _selectedCredential;

    [ObservableProperty]
    private string _searchText;

    public ObservableCollection<string> TypeNames => Types?.Select(x => x.Name).ToObservableCollection() ?? new();

    [ObservableProperty]
    private string _selectedType;
    async partial void OnSelectedTypeChanged(string value)
    {
        var credentials = await _credentialEndpoint.GetUsersCredentialsAsync(_loggedInUser.Id);
        var mappedCredentials = credentials.Select(x => new CredentialDisplayModel(x)).ToList();

        if (value == "All")
        {
            Credentials = new(mappedCredentials);
            return;
        }

        var types = await _typeEndpoint.GetAllTypesAsync();
        var selectedType = types.Where(x => x.Name.Equals(value)).FirstOrDefault();

        Credentials = mappedCredentials.Where(x => x.Type.Id == selectedType.Id).ToObservableCollection();
    }

    [RelayCommand]
    private async Task LoadCredentialsAsync()
    {
        var loadedCredentials = await _credentialEndpoint.GetUsersCredentialsAsync(_loggedInUser.Id);
        Credentials = new(loadedCredentials.Select(x => new CredentialDisplayModel(x)));
    }

    [RelayCommand]
    private async Task LoadTypesAsync()
    {
        var baseTypes = new List<TypeModel>
        {
            new TypeModel
            {
                Name = "All",
                Description = "Shows all the credentials."
            }
        };

        var loadedTypes = await _typeEndpoint.GetAllTypesAsync();

        Types = new(baseTypes.Concat(loadedTypes));
    }

    [RelayCommand]
    private static void CredentialClick(CredentialDisplayModel credential)
    {
        var message = new ViewCredentialMessage(credential);
        WeakReferenceMessenger.Default.Send(message);
    }

    [RelayCommand]
    private async Task PreferencesAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(SettingsPage)}?sub=appearance");
    }

    [RelayCommand]
    private async Task SearchCredentialsAsync()
    {
        await LoadCredentialsAsync();

        if (string.IsNullOrWhiteSpace(SearchText) is false)
        {
            Credentials = Credentials.OrderByDescending(x => x.Title.Contains(SearchText)).ToObservableCollection();
        }
    }

    [RelayCommand]
    private static void CreateCredentialPopup()
    {
        var message = new OpenCreateCredentialPopup(true);
        WeakReferenceMessenger.Default.Send(message);
    }
}
