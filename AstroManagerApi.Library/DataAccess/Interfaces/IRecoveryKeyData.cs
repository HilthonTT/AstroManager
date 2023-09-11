using AstroManagerApi.Library.Models;
using AstroManagerApi.Library.Models.Request;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IRecoveryKeyData
{
    Task<RecoveryRequestModel> CreateRecoveryKeysAsync(string userId);
    Task<List<RecoveryKeyModel>> GetUsersRecoveryKeysAsync(string userId);
}