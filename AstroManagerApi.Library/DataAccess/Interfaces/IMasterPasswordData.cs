using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IMasterPasswordData
{
    Task CreateMasterPasswordAsync(MasterPasswordModel password);
    Task<MasterPasswordModel> GetUsersMasterPasswordAsync(string userId);
    Task UpdateMasterPasswordAsync(MasterPasswordModel password);
}