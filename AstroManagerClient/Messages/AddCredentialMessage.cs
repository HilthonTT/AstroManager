using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AstroManagerClient.Messages;
public class AddCredentialMessage : ValueChangedMessage<bool>
{
    public AddCredentialMessage(bool value) : base(value)
    {
    }
}
