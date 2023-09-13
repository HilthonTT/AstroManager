using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Library.Api.Interfaces;
public interface IRecoveryKeyEndpoint
{
    Task<RecoveryRequestModel> CreateRecoveryKeyAsync(UserModel user);
    Task<RecoveryKeyModel> GetUsersRecoveryKeyAsync(string userId);
}