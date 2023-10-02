using AstroManagerApi.Library.DataAccess.Interfaces;
using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Models;
using BC = BCrypt.Net.BCrypt;

namespace AstroManagerApi.Library.Encryption;
public class TextHasher : ITextHasher
{
    private readonly IMasterPasswordData _passwordData;

    public TextHasher(IMasterPasswordData passwordData)
    {
        _passwordData = passwordData;
    }

    public MasterPasswordModel HashMasterPassword(MasterPasswordModel master)
    {
        string salt = BC.GenerateSalt();
        string hashedPassword = BC.HashPassword(master.HashedPassword, salt);

        master.Salt = salt;
        master.HashedPassword = hashedPassword;

        return master;
    }

    public async Task<bool> VerifyPasswordAsync(string userId, string password)
    {
        var masterPassword = await _passwordData.GetUsersMasterPasswordAsync(userId);
        if (masterPassword is null || string.IsNullOrWhiteSpace(masterPassword.Salt))
        {
            return false;
        }

        string saltedPassword = BC.HashPassword(password, masterPassword.Salt);

        return saltedPassword.Equals(masterPassword.HashedPassword);
    }

    public string HashPlainText(string plainText)
    {
        string salt = BC.GenerateSalt();

        return BC.HashPassword(plainText, salt);
    }

    public bool VerifyPassword(string plainText, string hashedText)
    {
        return BC.Verify(plainText, hashedText);
    }
}
