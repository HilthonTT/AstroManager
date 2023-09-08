namespace AstroManagerApi.Library.Encryption;

public interface IAesEncryptor
{
    Task<string> DecryptAsync(string cipherText);
    Task<string> EncryptAsync(string plainText);
}