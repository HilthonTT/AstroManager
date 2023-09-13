namespace AstroManagerApi.Library.Models.Request;
public class RecoveryRequestModel
{
    public RecoveryKeyModel Recovery { get; set; }
    public List<string> PlainRecoveryKeys { get; set; } = new();
}
