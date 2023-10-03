using CommunityToolkit.Mvvm.ComponentModel;
using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Models;
public partial class FieldDisplayModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TranslatedName))]
    private string _name;

    [ObservableProperty]
    private string _value;

    [ObservableProperty]
    private bool _isReadOnly = true;

    [ObservableProperty]
    private bool _isPassword = true;

    public string TranslatedName => TranslateName();

    public FieldDisplayModel()
    {
        
    }

    public FieldDisplayModel(FieldModel field)
    {
        Name = field.Name;
        Value = field.Value;
    }

    private string TranslateName()
    {
        string translatedValue = Name.ToLower() switch
        {
            "username" => LocalizationResourceManager.Instance["Username"].ToString(),
            "password" => LocalizationResourceManager.Instance["Password"].ToString(),
            "website" => LocalizationResourceManager.Instance["Website"].ToString(),
            "notes" => LocalizationResourceManager.Instance["Notes"].ToString(),
            "cardholder name" => LocalizationResourceManager.Instance["CardholderName"].ToString(),
            "verification number" => LocalizationResourceManager.Instance["VerificationNumber"].ToString(),
            "expiry date" => LocalizationResourceManager.Instance["ExpiryDate"].ToString(),
            "valid from" => LocalizationResourceManager.Instance["ValidFrom"].ToString(),
            "first name" => LocalizationResourceManager.Instance["FirstName"].ToString(),
            "last name" => LocalizationResourceManager.Instance["LastName"].ToString(),
            "gender" => LocalizationResourceManager.Instance["Gender"].ToString(),
            "birth date" => LocalizationResourceManager.Instance["BirthDate"].ToString(),
            "occupation" => LocalizationResourceManager.Instance["Occupation"].ToString(),
            "company" => LocalizationResourceManager.Instance["Company"].ToString(),
            "department" => LocalizationResourceManager.Instance["Department"].ToString(),
            _ => Name,
        };

        if (string.IsNullOrWhiteSpace(translatedValue))
        {
            return Name;
        }

        return translatedValue;
    }
}
