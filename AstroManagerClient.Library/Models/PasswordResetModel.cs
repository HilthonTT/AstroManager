namespace AstroManagerClient.Library.Models;
public class PasswordResetModel

{
    public MasterPasswordModel Master { get; set; } = new();
    public HashSet<string> RecoveryKeys { get; set; } = new();
}
