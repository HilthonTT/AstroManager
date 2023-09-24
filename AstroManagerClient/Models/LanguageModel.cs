using CommunityToolkit.Mvvm.ComponentModel;

namespace AstroManagerClient.Models;
public partial class LanguageModel : ObservableObject
{
    [ObservableProperty]
    private string _language;
}
