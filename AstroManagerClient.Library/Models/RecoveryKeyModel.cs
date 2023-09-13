namespace AstroManagerClient.Library.Models;
public class RecoveryKeyModel
{
    public string Id { get; set; }
    public BasicUserModel User { get; set; }
    public HashSet<string> RecoveryKeys { get; set; } = new();
}
