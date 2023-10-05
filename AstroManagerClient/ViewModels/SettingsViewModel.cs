namespace AstroManagerClient.ViewModels;
public partial class SettingsViewModel : BaseViewModel
{
    private static readonly Color PrimaryColor = Color.FromArgb("#6983EA");
    private static readonly Color DarkBg2Brush = Color.FromArgb("#1F1D2B");
    private readonly IErrorDisplayModel _error;
    private readonly ILoggedInUser _loggedInUser;
    private readonly IRecoveryKeyEndpoint _recoveryKeyEndpoint;

    public SettingsViewModel(
        IErrorDisplayModel error,
        ILoggedInUser loggedInUser,
        IRecoveryKeyEndpoint recoveryKeyEndpoint)
    {
        _error = error;
        _loggedInUser = loggedInUser;
        _recoveryKeyEndpoint = recoveryKeyEndpoint;

        LoadLanguages();
        LoadThemes();

        User = (UserModel)_loggedInUser;
    }

    [ObservableProperty]
    private string _selectedLanguage;

    [ObservableProperty]
    private UserModel _user;

    [ObservableProperty]
    private ObservableCollection<LanguageModel> _languages;

    [ObservableProperty]
    private ObservableCollection<ThemeModel> _themes;

    private void LoadLanguages()
    {
        string currentLanguageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        var languages = new ObservableCollection<LanguageModel>()
        {
            new() { Language = "English", Color = GetColorForLanguage("en", currentLanguageCode) },
            new() { Language = "Français", Color = GetColorForLanguage("fr", currentLanguageCode) },
            new() { Language = "Deutsch", Color = GetColorForLanguage("de", currentLanguageCode) },
            new() { Language = "Bahasa Indonesia", Color = GetColorForLanguage("id", currentLanguageCode) },
        };

        Languages = new(languages);
    }

    private void LoadThemes()
    {
        var themes = new ObservableCollection<ThemeModel>()
        {
            new() { Theme = "Dark Mode", Color = PrimaryColor },
            new() { Theme = "Light Mode" },
        };

        Themes = new(themes);
    }

    private static Color GetColorForLanguage(string languageCode, string selectedLanguageCode)
    {
        return languageCode.Equals(selectedLanguageCode, StringComparison.OrdinalIgnoreCase) ? PrimaryColor : DarkBg2Brush;
    }

    [RelayCommand]
    private async Task LoadRecoveryKeysAsync()
    {
        try
        {
            var recovery = await _recoveryKeyEndpoint.GetUsersRecoveryKeyAsync(_loggedInUser.Id);
            var mappedRecovery = new RecoveryKeyDisplayModel(recovery);

            var message = new ViewRecoveryMessage(mappedRecovery);
            WeakReferenceMessenger.Default.Send(message);
        }
        catch (Exception ex)
        {
            _error.SetErrorMessage(ex.Message);
            OpenErrorPopup();
        }
    }

    [RelayCommand]
    private void ChangeCulture(LanguageModel language)
    {
        // Set the color for the selected language
        language.Color = PrimaryColor;

        var languageCultureMap = new Dictionary<string, string>
        {
            { "English", "en-US" },
            { "Français", "fr-FR" }, // French (France) in native language
            { "Deutsch", "de-DE" },  // German (Germany) in native language
            { "Bahasa Indonesia", "id-ID" }, // Indonesian (Indonesia) in native language
        };

        if (languageCultureMap.TryGetValue(language.Language, out string cultureCode))
        {
            var cultureToSwitch = new CultureInfo(cultureCode);
            LocalizationResourceManager.Instance.SetCulture(cultureToSwitch);
        }

        // Set colors for all languages based on selection
        foreach (var lang in Languages)
        {
            lang.Color = lang.Language == language.Language ? PrimaryColor : DarkBg2Brush;
        }
    }

    [RelayCommand]
    private void ChangeTheme(ThemeModel theme)
    {
        Application.Current.UserAppTheme = theme.Theme.ToLower() switch
        {
            "dark mode" => AppTheme.Dark,
            _ => AppTheme.Light,
        };

        theme.Color = PrimaryColor;

        foreach (var t in Themes)
        {
            t.Color = t.Theme == theme.Theme ? PrimaryColor : DarkBg2Brush;
        }
    }

    [RelayCommand]
    private static void OpenSettingsDialog()
    {
        var message = new OpenAccountPopupMessage(true);
        WeakReferenceMessenger.Default.Send(message);
    }
}
