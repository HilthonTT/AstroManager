namespace AstroManagerClient.Library.Api.Interfaces;

public interface IWeakPasswordEndpoint
{
    Task<List<string>> GetWeakPasswordAsync();
}