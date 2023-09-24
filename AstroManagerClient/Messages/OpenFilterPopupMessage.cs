using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AstroManagerClient.Messages;
public class OpenFilterPopupMessage : ValueChangedMessage<bool>
{
    public OpenFilterPopupMessage(bool value) : base(value)
    {
    }
}
