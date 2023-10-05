namespace AstroManagerClient.Models;
public partial class KeyValueModel : ObservableObject
{
    [ObservableProperty]
    private int _keyIndex;

    [ObservableProperty]
    private string _value;
}
