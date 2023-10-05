namespace AstroManagerClient.Messages;
public class ViewRecoveryMessage : ValueChangedMessage<RecoveryKeyDisplayModel>
{
    public ViewRecoveryMessage(RecoveryKeyDisplayModel value) : base(value)
    {
    }
}
