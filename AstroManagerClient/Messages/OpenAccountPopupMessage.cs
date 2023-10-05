namespace AstroManagerClient.Messages;
public class OpenAccountPopupMessage : ValueChangedMessage<bool>
{
    public OpenAccountPopupMessage(bool value) : base(value)
    {
    }
}
