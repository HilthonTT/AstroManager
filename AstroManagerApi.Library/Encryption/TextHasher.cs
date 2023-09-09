using AstroManagerApi.Library.Encryption.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace AstroManagerApi.Library.Encryption;
public class TextHasher : ITextHasher
{
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
