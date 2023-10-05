namespace AstroManagerClient.Messages;
public class CloseFilterPopupMessage : ValueChangedMessage<TypeModel>
{
    public CloseFilterPopupMessage(TypeModel value) : base(value)
    {
    }
}
