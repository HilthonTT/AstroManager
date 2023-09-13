using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Library.Api.Interfaces;
public interface IUserEndpoint
{
    Task<UserModel> CreateUserAsync(UserModel user);
    Task<List<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserAsync(string id);
    Task<UserModel> GetUserFromAuthAsync(string oid);
    Task<string> UpdateUserAsync(UserModel user);
}