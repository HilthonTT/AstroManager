using AstroManagerClient.Models.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AstroManagerClient.Models;
public partial class ErrorDisplayModel : ObservableObject, IErrorDisplayModel
{
    [ObservableProperty]
    private string _errorMessage;
}
