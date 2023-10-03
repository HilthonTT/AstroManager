using CommunityToolkit.Mvvm.ComponentModel;
using AstroManagerClient.Library.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;

namespace AstroManagerClient.Models;
public partial class CredentialDisplayModel : ObservableObject
{
    public string Id { get; set; }

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private ObservableCollection<FieldDisplayModel> _fields;

    [ObservableProperty]
    private string _notes;

    public string ImageSource => GetImageSource();
    public string FavoriteImageSource => Favorited ? "star.png" : "star_black.png";

    public TypeModel Type { get; set; }
    public BasicUserModel User { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.UtcNow;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FavoriteImageSource))]
    private bool _favorited;

    [ObservableProperty]
    private DateTime _dateModified;

    public CredentialDisplayModel()
    {
        
    }

    public CredentialDisplayModel(CredentialModel credential)
    {
        Id = credential.Id;
        Title = credential.Title;
        Fields = credential.Fields.Select(x => new FieldDisplayModel(x)).ToObservableCollection();
        Type = credential.Type;
        User = credential.User;
        DateAdded = credential.DateAdded;
        DateModified = credential.DateModified;
        Favorited = credential.Favorited;
    }

    private string GetImageSource()
    {
        return Type.Name.ToLower() switch
        {
            "logins" => "login.png",
            "passwords" => "password.png",
            "secure notes" or "secure note" => "secure_note.png",
            "credit card" or "creditcard" => "credit_card.png",
            "identity" => "identity.png",
            _ => "astronaut.png"
        };
    }
}
