using AstroManagerApi.Library.Models;
using AstroManagerApi.Library.Models.Request;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IRecoveryKeyData
{
    Task<RecoveryRequestModel> CreateRecoveryKeysAsync(UserModel user);
    Task<RecoveryKeyModel> GetUsersRecoveryKeyAsync(string userId);
}