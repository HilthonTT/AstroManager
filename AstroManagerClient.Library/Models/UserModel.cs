using AstroManagerClient.Library.Models.Interfaces;

namespace AstroManagerClient.Library.Models;
public class UserModel : ILoggedInUser
{
    public string Id { get; set; }
    public string ObjectIdentifier { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
}
