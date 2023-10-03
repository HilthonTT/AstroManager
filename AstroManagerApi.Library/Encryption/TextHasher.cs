using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Models;
using BC = BCrypt.Net.BCrypt;

namespace AstroManagerApi.Library.Encryption;
public class TextHasher : ITextHasher
{
    public MasterPasswordModel HashMasterPassword(MasterPasswordModel master)
    {
        string salt = BC.GenerateSalt();
        string hashedPassword = BC.HashPassword(master.HashedPassword, salt);

        master.Salt = salt;
        master.HashedPassword = hashedPassword;

        return master;
    }

    public RecoveryKeyModel HashRecoveryKeys(RecoveryKeyModel recoveryKey)
    {
        var hashedKeys = new HashSet<string>();
        string salt = BC.GenerateSalt();

        recoveryKey.Salt = salt;

        Parallel.ForEach(recoveryKey.RecoveryKeys, k =>
        {
            hashedKeys.Add(BC.HashPassword(k, salt));
        });

        recoveryKey.RecoveryKeys = hashedKeys;

        return recoveryKey;
    }

    public bool VerifyPassword(string password, MasterPasswordModel masterPassword)
    {
        if (masterPassword is null || string.IsNullOrWhiteSpace(masterPassword.Salt))
        {
            return false;
        }

        string saltedPassword = BC.HashPassword(password, masterPassword.Salt);

        return saltedPassword.Equals(masterPassword.HashedPassword);
    }
}
