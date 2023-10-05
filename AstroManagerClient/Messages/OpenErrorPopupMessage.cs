namespace AstroManagerClient.Messages;
public class OpenErrorPopupMessage : ValueChangedMessage<bool>
{
    public OpenErrorPopupMessage(bool value) : base(value)
    {
    }
}
