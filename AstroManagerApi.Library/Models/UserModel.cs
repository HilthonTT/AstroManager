using System.ComponentModel.DataAnnotations;

namespace AstroManagerApi.Library.Models;
public class UserModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The oid is required.")]
    [StringLength(36, ErrorMessage = "Your oid is too long.")]
    public string ObjectIdentifier { get; set; }

    [Required(ErrorMessage = "The first name is required.")]
    [StringLength(50, ErrorMessage = "Your first name is too long.")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "The last name is required.")]
    [StringLength(50, ErrorMessage = "Your last name is too long.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "The email address is required.")]
    [StringLength(256, ErrorMessage = "Your email address too long.")]
    public string EmailAddress { get; set; }
}
