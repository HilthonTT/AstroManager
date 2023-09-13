using CommunityToolkit.Mvvm.ComponentModel;

namespace AstroManagerClient.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;
}
