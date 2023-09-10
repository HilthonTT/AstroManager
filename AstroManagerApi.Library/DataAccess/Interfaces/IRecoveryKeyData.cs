using AstroManagerApi.Library.Models;
using AstroManagerApi.Library.Models.Request;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IRecoveryKeyData
{
    Task<RecoveryRequestModel> CreateRecoveryKeysAsync(int userId);
    Task<List<RecoveryKeyModel>> GetRecoveryKeysByUserIdAsync(int userId);
}