﻿namespace AstroManagerClient.Messages;
public class OpenCreateCredentialMessage : ValueChangedMessage<bool>
{
    public OpenCreateCredentialMessage(bool value) : base(value)
    {
    }
}
