using AstroManagerApi.Library.Models.Request;

namespace AstroManagerApi.Library.Encryption.Interfaces;
public interface IRecoveryKeyGenerator
{
    RecoveryRequestModel GenerateRequest();
    bool VerifyKey(string key, string hashedKey);
}