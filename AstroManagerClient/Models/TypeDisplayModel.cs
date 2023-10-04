using CommunityToolkit.Mvvm.ComponentModel;
using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Models;
public partial class TypeDisplayModel : ObservableObject
{
    public string Id { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TranslatedName))]
    private string _name;

    [ObservableProperty]
    private string _description;

    public string TranslatedName => GetTranslatedName();

    public TypeDisplayModel()
    {
        
    }

    public TypeDisplayModel(TypeModel type)
    {
        Id = type.Id;
        Name = type.Name;
        Description = type.Description;
    }

    private string GetTranslatedName()
    {
        return Name.ToLower() switch
        {
            "all" => LocalizationResourceManager.Instance["All"].ToString(),
            "logins" => LocalizationResourceManager.Instance["Logins"].ToString(),
            "passwords" => LocalizationResourceManager.Instance["Passwords"].ToString(),
            "secure note" => LocalizationResourceManager.Instance["SecureNote"].ToString(),
            "credit card" => LocalizationResourceManager.Instance["CreditCard"].ToString(),
            "identity" => LocalizationResourceManager.Instance["Identity"].ToString(),
            _ => Name,
        };
    }
}
