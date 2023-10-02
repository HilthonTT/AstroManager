using AstroManagerApi.Library.Models;

namespace AstroManagerApi.Library.Encryption.Interfaces;

public interface ITextHasher
{
    MasterPasswordModel HashMasterPassword(MasterPasswordModel master);
    string HashPlainText(string plainText);
    bool VerifyPassword(string plainText, string hashedText);
    Task<bool> VerifyPasswordAsync(string userId, string password);
}