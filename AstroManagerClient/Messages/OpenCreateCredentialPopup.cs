using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AstroManagerClient.Messages;
public class OpenCreateCredentialPopup : ValueChangedMessage<bool>
{
    public OpenCreateCredentialPopup(bool value) : base(value)
    {
    }
}
