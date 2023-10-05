namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IRecoveryKeyData
{
    Task<RecoveryKeyModel> CreateRecoveryKeysAsync(UserModel user);
    Task<RecoveryKeyModel> GetUsersRecoveryKeyAsync(string userId);
}