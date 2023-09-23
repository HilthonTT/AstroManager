﻿using AstroManagerClient.ConstantsVariables;
using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Library.Models.Interfaces;
using AstroManagerClient.Library.Storage.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class SettingsViewModel : BaseViewModel
{
    private static readonly TimeSpan ThemeCacheTimeSpan = TimeSpan.FromDays(365 * 50);
    private readonly ILoggedInUser _loggedInUser;
    private readonly IRecoveryKeyEndpoint _recoveryKeyEndpoint;
    private readonly IUserEndpoint _userEndpoint;
    private readonly ISecureStorageWrapper _storage;

    public SettingsViewModel(
        ILoggedInUser loggedInUser,
        IRecoveryKeyEndpoint recoveryKeyEndpoint,
        IUserEndpoint userEndpoint,
        ISecureStorageWrapper storage)
    {
        _loggedInUser = loggedInUser;
        _recoveryKeyEndpoint = recoveryKeyEndpoint;
        _userEndpoint = userEndpoint;
        _storage = storage;
    }

    [ObservableProperty]
    private bool _editProfile;

    [ObservableProperty]
    private EditUserModel _model;

    [RelayCommand]
    private async Task LoadRecoveryKeysAsync()
    {
        var recovery = await _recoveryKeyEndpoint.GetUsersRecoveryKeyAsync(_loggedInUser.Id);
        var mappedRecovery = new RecoveryKeyDisplayModel(recovery);

        var message = new ViewRecoveryMessage(mappedRecovery);
        WeakReferenceMessenger.Default.Send(message);
    }

    [RelayCommand]
    private async Task EditDisplayNameAsync()
    {
        _loggedInUser.DisplayName = Model.DisplayName;

        try
        {
            IsBusy = true;
            await _userEndpoint.UpdateUserAsync(_loggedInUser as UserModel);
        }
        catch (Exception)
        {
            //TODO: Go to error page
        }
        finally
        {
            IsBusy = false;
            EditProfile = false;
        }
    }

    [RelayCommand]
    private async Task ChangeThemeAsync(string theme)
    {
        Application.Current.UserAppTheme = theme switch
        {
            Constants.DarkTheme => AppTheme.Dark,
            Constants.LightTheme => AppTheme.Light,
            _ => AppTheme.Dark,
        };

        await _storage.SetRecordAsync(Constants.ThemeKey, theme, ThemeCacheTimeSpan);
    }
}
