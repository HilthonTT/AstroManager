using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AstroManagerClient.Messages;
public class UserLoggedInMessage : ValueChangedMessage<bool>
{
    public UserLoggedInMessage(bool value) : base(value)
    {
    }
}
