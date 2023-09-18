namespace AstroManagerClient.Library.Models;
public class BasicUserModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public BasicUserModel()
    {
        
    }

    public BasicUserModel(UserModel user)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
    }
}
