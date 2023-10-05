namespace AstroManagerClient.Messages;
public class ViewCredentialMessage : ValueChangedMessage<CredentialDisplayModel>
{
    public ViewCredentialMessage(CredentialDisplayModel value) : base(value)
    {
    }
}
