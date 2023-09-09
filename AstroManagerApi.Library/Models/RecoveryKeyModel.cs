namespace AstroManagerApi.Library.Models;
public class RecoveryKeyModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string HashedKey { get; set; }
}
