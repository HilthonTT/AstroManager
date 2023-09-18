using AstroManagerClient.Library.Models;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AstroManagerClient.Models;
public partial class TemplateDisplayModel : ObservableObject
{
    public string Id { get; set; }
    public TypeModel Type { get; set; }
    public ObservableCollection<FieldDisplayModel> Fields { get; set; } = new();

    public TemplateDisplayModel()
    {
        
    }

    public TemplateDisplayModel(CredentialTemplateModel template)
    {
        Id = template.Id;
        Type = template.Type;
        Fields = template.Fields.Select(x => new FieldDisplayModel(x)).ToObservableCollection();
    }
}
