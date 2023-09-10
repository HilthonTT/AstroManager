namespace AstroManagerApi.Models;

public class UpdatePasswordRequestModel
{
    public int UserId { get; set; }
    public string NewPassword { get; set; }
    public List<string> RecoveryKeys { get; set; } = new();
}
