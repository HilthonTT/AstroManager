namespace AstroManagerClient.Library.Storage;
public class CachedData<T>
{
    public T Data { get; set; }
    public DateTimeOffset Expiration { get; set; }
}
