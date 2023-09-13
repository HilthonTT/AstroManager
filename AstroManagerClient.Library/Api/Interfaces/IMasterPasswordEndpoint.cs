using AstroManagerClient.Library.Models;

namespace AstroManagerClient.Library.Api.Interfaces;
public interface IMasterPasswordEndpoint
{
    Task<MasterPasswordModel> CreateMasterPasswordAsync(MasterPasswordModel password);
    Task<MasterPasswordModel> GetUsersMasterPasswordAsync(string userId);
    Task<string> UpdateMasterPasswordAsync(PasswordResetModel password);
}