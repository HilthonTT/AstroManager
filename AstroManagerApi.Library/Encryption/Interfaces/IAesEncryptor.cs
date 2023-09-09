namespace AstroManagerApi.Library.Encryption.Interfaces;

public interface IAesEncryptor
{
    Task<string> DecryptAsync(string cipherText);
    Task<string> EncryptAsync(string plainText);
}