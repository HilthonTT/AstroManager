﻿namespace AstroManagerClient.Library.Api.Interfaces;

public interface IApiHelper
{
    HttpClient HttpClient { get; }

    void AcquireHeaders(string token);
}