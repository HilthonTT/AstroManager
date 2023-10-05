namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IMasterPasswordData
{
    Task<MasterPasswordModel> CreateMasterPasswordAsync(MasterPasswordModel password);
    Task<MasterPasswordModel> GetUsersMasterPasswordAsync(string userId);
    Task UpdateMasterPasswordAsync(MasterPasswordModel password);
}