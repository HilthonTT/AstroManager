using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IUserData
{
    Task CreateUserAsync(UserModel user);
    Task<UserModel> GetUserByOidAsync(string oid);
    Task UpdateUserAsync(UserModel user);
}