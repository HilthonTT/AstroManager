﻿using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Globalization;

namespace AstroManagerClient.ViewModels;
public partial class SettingsViewModel : BaseViewModel
{
    private static readonly Color PrimaryColor = Color.FromArgb("#EA7C69");
    private static readonly Color DarkBg2Brush = Color.FromArgb("#1F1D2B");
    private readonly ILoggedInUser _loggedInUser;
    private readonly IRecoveryKeyEndpoint _recoveryKeyEndpoint;

    public SettingsViewModel(
        ILoggedInUser loggedInUser,
        IRecoveryKeyEndpoint recoveryKeyEndpoint)
    {
        _loggedInUser = loggedInUser;
        _recoveryKeyEndpoint = recoveryKeyEndpoint;

        LoadLanguages();
    }

    [ObservableProperty]
    private string _selectedLanguage;

    [ObservableProperty]
    private ObservableCollection<LanguageModel> _languages;

    private void LoadLanguages()
    {
        var languages = new ObservableCollection<LanguageModel>()
        {
            new() { Language = "English-US", Color = PrimaryColor },
            new() { Language = "French-FR" },
        };

        Languages = new(languages);
    }

    [RelayCommand]
    private async Task LoadRecoveryKeysAsync()
    {
        var recovery = await _recoveryKeyEndpoint.GetUsersRecoveryKeyAsync(_loggedInUser.Id);
        var mappedRecovery = new RecoveryKeyDisplayModel(recovery);

        var message = new ViewRecoveryMessage(mappedRecovery);
        WeakReferenceMessenger.Default.Send(message);
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
}
