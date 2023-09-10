using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IUserData
{
    Task CreateUserAsync(UserModel user);
    Task<List<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserAsync(string id);
    Task<UserModel> GetUserFromAuthAsync(string objectId);
    Task UpdateUserAsync(UserModel user);
}