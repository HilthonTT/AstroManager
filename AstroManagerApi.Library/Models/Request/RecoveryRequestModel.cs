namespace AstroManagerApi.Library.Models.Request;
public class RecoveryRequestModel
{
    public RecoveryKeyModel Recovery { get; set; }
    public List<string> PlainRecoveryKeys { get; set; } = new();

    public RecoveryRequestModel()
    {
        
    }

    public RecoveryRequestModel(RecoveryKeyModel recovery, List<string> plainKeys)
    {
        Recovery = recovery;
        PlainRecoveryKeys = plainKeys;
    }
}
