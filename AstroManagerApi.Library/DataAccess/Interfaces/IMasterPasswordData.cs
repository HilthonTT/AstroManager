using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.DataAccess.Interfaces;
public interface IMasterPasswordData
{
    Task CreateMasterPasswordAsync(MasterPasswordModel master);
    Task<MasterPasswordModel> GetMasterPasswordByUserIdAsync(int userId);
    Task UpdateMasterPasswordAsync(MasterPasswordModel master);
}