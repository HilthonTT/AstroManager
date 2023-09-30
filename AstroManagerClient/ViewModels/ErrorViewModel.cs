using AstroManagerClient.Messages;
using AstroManagerClient.Models;
using AstroManagerClient.Models.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AstroManagerClient.ViewModels;
public partial class ErrorViewModel : BaseViewModel
{
    private readonly IErrorDisplayModel _errorMessage;
    public ErrorViewModel()
    {
        _errorMessage = App.Services.GetService<IErrorDisplayModel>();

        Error = (ErrorDisplayModel)_errorMessage;
    }

    [ObservableProperty]
    private ErrorDisplayModel _error;

    [RelayCommand]
    private void Close()
    {
        _errorMessage.ErrorMessage = "";

        var message = new OpenErrorPopupMessage(false);
        WeakReferenceMessenger.Default.Send(message);
    }
}
