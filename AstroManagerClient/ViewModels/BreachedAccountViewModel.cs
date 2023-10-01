using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using AstroManagerClient.Models.Interfaces;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace AstroManagerClient.ViewModels;
public partial class BreachedAccountViewModel : BaseViewModel
{
    private readonly IErrorDisplayModel _error;
    private readonly IPasswordBreacherEndpoint _breacherEndpoint;
    private readonly ILoggedInUser _loggedInUser;
    private readonly ICredentialEndpoint _credentialEndpoint;
    private readonly ITypeEndpoint _typeEndpoint;

    public BreachedAccountViewModel(
        IErrorDisplayModel error,
        IPasswordBreacherEndpoint breacherEndpoint,
        ILoggedInUser loggedInUser,
        ICredentialEndpoint credentialEndpoint,
        ITypeEndpoint typeEndpoint)
    {
        _error = error;
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
    private async Task LoadDataAsync()
    {
        try
        {
            await LoadBreachedCredentialsAsync();
            await LoadTypesAsync();
        }
        catch (Exception ex)
        {
            _error.SetErrorMessage(ex.Message);
            OpenErrorPopup();
        }
    }

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
            _error.SetErrorMessage(ex.Message);
            OpenErrorPopup();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadTypesAsync()
    {
        try
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
        catch (Exception ex)
        {
            _error.SetErrorMessage(ex.Message);
            OpenErrorPopup();
        }
    }

    [RelayCommand]
    private static void CredentialClick(CredentialDisplayModel credential)
    {
        var message = new ViewCredentialMessage(credential);
        WeakReferenceMessenger.Default.Send(message);
    }
}
