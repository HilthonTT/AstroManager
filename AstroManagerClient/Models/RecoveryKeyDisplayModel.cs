using AstroManagerClient.Library.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AstroManagerClient.Models;
public partial class RecoveryKeyDisplayModel : ObservableObject
{
    public string Id { get; set; }
    public BasicUserModel User { get; set; }

    [ObservableProperty]
    private HashSet<string> _recoveryKeys = new();

    public RecoveryKeyDisplayModel()
    {
        
    }

    public RecoveryKeyDisplayModel(RecoveryKeyModel recovery)
    {
        Id = recovery.Id;
        User = recovery.User;
        RecoveryKeys = recovery.RecoveryKeys;
    }
}
