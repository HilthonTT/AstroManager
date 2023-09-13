namespace AstroManagerClient.Library.Models;
public class PasswordResetModel
{
    public MasterPasswordModel Master { get; set; }
    public HashSet<string> RecoveryKeys { get; set; }
}
