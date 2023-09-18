using AstroManagerClient.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace AstroManagerClient.Messages;
public class ViewCredentialMessage : ValueChangedMessage<CredentialDisplayModel>
{
    public ViewCredentialMessage(CredentialDisplayModel value) : base(value)
    {
    }
}
