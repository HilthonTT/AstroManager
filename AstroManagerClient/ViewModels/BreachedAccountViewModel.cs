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
    private ObservableCollection<TypeDisplayModel> _types = new();

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

    public ObservableCollection<string> TypeNames => Types?.Select(x => x.TranslatedName).ToObservableCollection();

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

            Types = new(baseTypes.Concat(loadedTypes).Select(x => new TypeDisplayModel(x)));
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

    [RelayCommand]
    private async Task FavoriteCredentialAsync(CredentialDisplayModel credential)
    {
        try
        {
            credential.Favorited = !credential.Favorited;
            var mappedCredential = ModelConverter.GetCredential(credential);

            await _credentialEndpoint.UpdateCredentialAsync(mappedCredential);
        }
        catch (Exception ex)
        {
            _error.SetErrorMessage(ex.Message);
            OpenErrorPopup();
        }
    }
}
