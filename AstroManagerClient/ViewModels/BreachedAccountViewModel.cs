using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace AstroManagerClient.ViewModels;
public partial class BreachedAccountViewModel : BaseViewModel
{
    private readonly IPasswordBreacherEndpoint _breacherEndpoint;
    private readonly ILoggedInUser _loggedInUser;
    private readonly ICredentialEndpoint _credentialEndpoint;
    private readonly ITypeEndpoint _typeEndpoint;

    public BreachedAccountViewModel(
        IPasswordBreacherEndpoint breacherEndpoint,
        ILoggedInUser loggedInUser,
        ICredentialEndpoint credentialEndpoint,
        ITypeEndpoint typeEndpoint)
    {
        _breacherEndpoint = breacherEndpoint;
        _loggedInUser = loggedInUser;
        _credentialEndpoint = credentialEndpoint;
        _typeEndpoint = typeEndpoint;
    }

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TypeNames))]
    private ObservableCollection<TypeModel> _types = new();

    [ObservableProperty]
    private ObservableCollection<CredentialDisplayModel> _credentials = new();

    [ObservableProperty]
    private string _selectedType;
    async partial void OnSelectedTypeChanged(string value)
    {
        await LoadBreachedCredentialsAsync();
        if (value.Equals("All", StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        var filteredCredentials = Credentials.Where(x => x.Type.Name == value).ToObservableCollection();
        Credentials = new(filteredCredentials);
    }

    public ObservableCollection<string> TypeNames => Types?.Select(x => x.Name).ToObservableCollection();

    [RelayCommand]
    private async Task LoadBreachedCredentialsAsync()
    {
        IsLoading = true;

        try
        {
            var credentials = await _credentialEndpoint.GetUsersCredentialsAsync(_loggedInUser.Id);
            var breachedCredentials = await _breacherEndpoint.GetBreachedCredentialsAsync(credentials);
            if (breachedCredentials is null)
            {
                return;
            }

            var mappedCredentials = breachedCredentials.Select(x => new CredentialDisplayModel(x));
            Credentials = new(mappedCredentials);
        }
        catch (Exception ex)
        {
            // Implement error message popup
        }
        finally
        {
            IsLoading = false;
        }
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
}
