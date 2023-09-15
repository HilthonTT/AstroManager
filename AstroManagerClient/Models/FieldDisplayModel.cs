using CommunityToolkit.Mvvm.ComponentModel;
using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Models;
public partial class FieldDisplayModel : ObservableObject
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _value;

    public FieldDisplayModel()
    {
        
    }

    public FieldDisplayModel(FieldModel field)
    {
        Name = field.Name;
        Value = field.Value;
    }
}
