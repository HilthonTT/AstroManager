namespace AstroManagerClient.Library.Models;
public class MasterPasswordModel
{
    public string Id { get; set; }
    public BasicUserModel User { get; set; }
    public string HashedPassword { get; set; }

    public MasterPasswordModel(MasterPasswordModel master)
    {
        Id = master.Id;
        User = master.User;
        HashedPassword = master.HashedPassword;
    }

    public MasterPasswordModel()
    {
        
    }
}
