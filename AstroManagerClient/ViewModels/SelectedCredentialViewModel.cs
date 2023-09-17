using AstroManagerClient.Library.Api;
using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class SelectedCredentialViewModel : BaseViewModel
{
    private readonly ICredentialEndpoint _credentialEndpoint;

    public SelectedCredentialViewModel()
    {
        _credentialEndpoint = App.Services.GetService<ICredentialEndpoint>();

        WeakReferenceMessenger.Default.Register<CredentialDisplayModel>(this, (r, m) =>
        {
            Credential = m;
        });
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCredential))]
    private CredentialDisplayModel _credential;

    [ObservableProperty]
    private string _selectedTab;

    partial void OnSelectedTabChanged(string value)
    {
        bool isReadonly = value switch
        {
            "Information" or "Export" => true,
            "Edit" => false,
            _ => true,
        };

        CanEdit = !isReadonly;

        if (isReadonly)
        {
            ChangeFieldsReadonly(true);
        }
        else
        {
            ChangeFieldsReadonly(false);
        }
    }

    [ObservableProperty]
    private bool _isPassword = true;

    [ObservableProperty]
    private bool _canEdit;

    public bool IsCredential => Credential is not null;

    private void ChangeFieldsReadonly(bool isReadonly)
    {
        foreach (var f in Credential.Fields)
        {
            f.IsReadOnly = isReadonly;
        }
    }

    private void ChangeFieldsPassword(bool isPassword)
    {
        foreach (var f in Credential.Fields)
        {
            f.IsPassword = isPassword;
        }
    }

    [RelayCommand]
    private void CloseCredential()
    {
        Credential = null;
    }

    [RelayCommand]
    private void ToggleShowPassword()
    {
        IsPassword = !IsPassword;
        ChangeFieldsPassword(IsPassword);
    }

    [RelayCommand]
    private async Task SaveChangesAsync()
    {
        var credential = ModelConverter.GetCredential(Credential);

        await _credentialEndpoint.UpdateCredentialAsync(credential);

        await Shell.Current.DisplayAlert("Saved Credential", "Your credential have been saved!", "OK");
    }
}
