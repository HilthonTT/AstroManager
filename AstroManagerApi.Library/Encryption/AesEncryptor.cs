using AstroManagerApi.Library.Encryption.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace AstroManagerApi.Library.Encryption;
public class AesEncryptor : IAesEncryptor
{
    private readonly Aes _aes;
    private readonly IConfiguration _config;

    public AesEncryptor(IConfiguration config)
    {
        _config = config;

        _aes = Aes.Create();
        _aes.Key = StringToBytes(_config["Aes:KEY"]);
        _aes.IV = StringToBytes(_config["Aes:IV"]);
    }

    private static byte[] StringToBytes(string text)
    {
        return Convert.FromBase64String(text);
    }

    public async Task<string> EncryptAsync(string plainText)
    {
        byte[] encrypted;

        using (ICryptoTransform encryptor = _aes.CreateEncryptor())
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                await csEncrypt.WriteAsync(plainBytes, 0, plainBytes.Length);
                await csEncrypt.FlushFinalBlockAsync();
            }

            encrypted = msEncrypt.ToArray();
        }

        return Convert.ToBase64String(encrypted);
    }

    public async Task<string> DecryptAsync(string cipherText)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using ICryptoTransform decryptor = _aes.CreateDecryptor();
        byte[] decryptedBytes;

        using (var msDecrypt = new MemoryStream(cipherBytes))
        {
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var ms = new MemoryStream();
            await csDecrypt.CopyToAsync(ms);
            decryptedBytes = ms.ToArray();
        }

        string decryptedText = Encoding.UTF8.GetString(decryptedBytes).TrimEnd('\0');

        return decryptedText;
    }
}
