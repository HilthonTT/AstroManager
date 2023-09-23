using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AstroManagerClient.Messages;
public class ViewRecoveryMessage : ValueChangedMessage<RecoveryKeyDisplayModel>
{
    public ViewRecoveryMessage(RecoveryKeyDisplayModel value) : base(value)
    {
    }
}
