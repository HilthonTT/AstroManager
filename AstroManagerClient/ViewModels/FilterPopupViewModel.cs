using AstroManagerClient.Library.Api.Interfaces;
using AstroManagerClient.Library.Models;
using AstroManagerClient.Messages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class FilterPopupViewModel : BaseViewModel
{
    private readonly ITypeEndpoint _typeEndpoint;
    public FilterPopupViewModel()
    {
        _typeEndpoint = App.Services.GetService<ITypeEndpoint>();
    }

    [ObservableProperty]
    private TypeModel _selectedType;

    [ObservableProperty]
    private string _type = "Logins";
    async partial void OnTypeChanged(string value)
    {
        var types = await _typeEndpoint.GetAllTypesAsync();
        SelectedType = types.FirstOrDefault(x => x.Name.Contains(Type));
    }

    [RelayCommand]
    private void FilterCredential()
    {
        SelectedType ??= new()
        {
            Name = "All",
            Description = "Shows all the credentials."
        };

        var message = new CloseFilterPopupMessage(SelectedType);
        WeakReferenceMessenger.Default.Send(message);
    }
}
