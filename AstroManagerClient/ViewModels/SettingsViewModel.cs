using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using AstroManagerClient.Models.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Globalization;

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
        var languages = new ObservableCollection<LanguageModel>()
        {
            new() { Language = "English-US", Color = PrimaryColor },
            new() { Language = "French-FR" },
            new() { Language = "German-DE" },
            new() { Language = "Indonesian-ID" },
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
            _error.ErrorMessage = $"Something went wrong on our side. {ex.Message}";
            OpenErrorPopup();
        }
    }

    [RelayCommand]
    private void ChangeCulture(LanguageModel language)
    {
        string culture;

        if (language.Language == "English-US")
        {
            culture = "en-US";
        }
        else
        {
            culture = "fr-FR";
        }

        language.Color = PrimaryColor;

        var cultureToSwitch = new CultureInfo(culture);

        LocalizationResourceManager.Instance.SetCulture(cultureToSwitch);

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
