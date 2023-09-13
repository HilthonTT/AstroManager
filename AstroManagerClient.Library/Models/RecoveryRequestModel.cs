namespace AstroManagerClient.Library.Models;
public class RecoveryRequestModel
{
    public RecoveryKeyModel Recovery { get; set; }
    public List<string> PlainRecoveryKeys { get; set; } = new();
}
