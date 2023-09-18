using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Text.Json;

namespace AstroManagerClient.ViewModels;
public partial class SelectedCredentialViewModel : BaseViewModel
{
    private readonly ICredentialEndpoint _credentialEndpoint;

    public SelectedCredentialViewModel()
    {
        _credentialEndpoint = App.Services.GetService<ICredentialEndpoint>();

        WeakReferenceMessenger.Default.Register<ViewCredentialMessage>(this, (r, m) =>
        {
            Credential = m.Value;
        });

        WeakReferenceMessenger.Default.Register<AddCredentialMessage>(this, (r, m) =>
        {
            if (m.Value is false)
            {
                IsCreating = false;
                CloseCredential();
            }
        });
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCredential))]
    [NotifyPropertyChangedFor(nameof(IsNotCredential))]
    private CredentialDisplayModel _credential;

    [ObservableProperty]
    private string _selectedTab;

    [ObservableProperty]
    private bool _isPassword = true;

    [ObservableProperty]
    private bool _canEdit;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsCredential))]
    private bool _isCreating;

    public bool IsCredential => Credential is not null && IsCreating is false;
    public bool IsNotCredential => !IsCredential;

    async partial void OnSelectedTabChanged(string value)
    {
        bool isReadonly = value switch
        {
            "Information" or "Export" => true,
            "Edit" => false,
            _ => true,
        };

        CanEdit = !isReadonly;

        if (value == "Export")
        {
            await ExportDataAsync();
            return;
        }

        if (isReadonly)
        {
            ChangeFieldsReadonly(true);
        }
        else
        {
            ChangeFieldsReadonly(false);
        }
    }

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

    private async Task ExportDataAsync()
    {
        var credential = ModelConverter.GetCredential(Credential);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        string jsonCredential = JsonSerializer.Serialize(credential, options);

        var result = await FolderPicker.Default.PickAsync(new());
        if (result.IsSuccessful)
        {
            string fullPath = Path.Combine(result.Folder.Path, $"{credential.Id}.json");
            await File.WriteAllTextAsync(fullPath, jsonCredential);
        }
    }

    [RelayCommand]
    private void CloseCredential()
    {
        Credential = null;
        CanEdit = false;
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

    [RelayCommand]
    private void CreateCredential()
    {
        Credential = new();

        IsCreating = true;

        var message = new AddCredentialMessage(true);
        WeakReferenceMessenger.Default.Send(message);
    }
}
