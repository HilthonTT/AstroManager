using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.Encryption.Interfaces;

public interface ITextHasher
{
    MasterPasswordModel HashMasterPassword(MasterPasswordModel master);
    RecoveryKeyModel HashRecoveryKeys(RecoveryKeyModel recoveryKey);
    bool VerifyPassword(string password, MasterPasswordModel masterPassword);
}