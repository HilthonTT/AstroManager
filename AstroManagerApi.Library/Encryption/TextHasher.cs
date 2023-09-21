using AstroManagerApi.Library.Encryption.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace AstroManagerApi.Library.Encryption;
public class TextHasher : ITextHasher
{
    private const int Salt = 12;

    public string HashPlainText(string plainText)
    {
        return BC.HashPassword(plainText, Salt);
    }

    public bool VerifyPassword(string plainText, string hashedText)
    {
        return BC.Verify(plainText, hashedText);
    }
}
