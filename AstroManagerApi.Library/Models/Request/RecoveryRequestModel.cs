namespace AstroManagerApi.Library.Models.Request;
public class RecoveryRequestModel
{
    public List<string> PlainRecoveryKeys { get; set; } = new();
    public List<string> HashedRecoveryKeys { get; set; } = new();
}
