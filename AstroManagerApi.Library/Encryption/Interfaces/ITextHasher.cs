namespace AstroManagerApi.Library.Encryption.Interfaces;

public interface ITextHasher
{
    string HashPlainText(string plainText);
    bool VerifyPassword(string plainText, string hashedText);
}