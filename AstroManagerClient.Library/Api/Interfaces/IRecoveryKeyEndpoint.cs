using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Library.Api.Interfaces;
public interface IRecoveryKeyEndpoint
{
    Task<RecoveryKeyModel> CreateRecoveryKeyAsync(UserModel user);
    Task<RecoveryKeyModel> GetUsersRecoveryKeyAsync(string userId);
}