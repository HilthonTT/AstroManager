using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace AstroManagerApi.Library.Encryption;
public class AesEncryptor : IAesEncryptor
{
    private readonly string _key;
    private readonly string _iv;
    private readonly IConfiguration _config;

    public AesEncryptor(IConfiguration config)
    {
        _config = config;

        _key = _config["aes_key"];
        _iv = _config["aes_iv"];
    }

    public async Task<string> EncryptAsync(string plainText)
    {
        using var aesAlg = Aes.Create();

        aesAlg.Key = Encoding.UTF8.GetBytes(_key);
        aesAlg.IV = Encoding.UTF8.GetBytes(_iv);

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            await swEncrypt.WriteAsync(plainText);
        }

        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public async Task<string> DecryptAsync(string cipherText)
    {
        using var aesAlg = Aes.Create();

        aesAlg.Key = Encoding.UTF8.GetBytes(_key);
        aesAlg.IV = Encoding.UTF8.GetBytes(_iv);

        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

        using var srDecrypt = new StreamReader(csDecrypt);
        return await srDecrypt.ReadToEndAsync();
    }
}
