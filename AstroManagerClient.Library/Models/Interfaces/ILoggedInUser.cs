namespace AstroManagerClient.Library.Models.Interfaces;

public interface ILoggedInUser
{
    string EmailAddress { get; set; }
    string FirstName { get; set; }
    string Id { get; set; }
    string LastName { get; set; }
    string ObjectIdentifier { get; set; }
}