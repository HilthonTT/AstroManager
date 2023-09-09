namespace AstroManagerApi.Library.Models;
public class MasterPasswordModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string HashedPassword { get; set; }
}
