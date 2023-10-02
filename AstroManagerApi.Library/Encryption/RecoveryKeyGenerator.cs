using AstroManagerApi.Library.Encryption.Interfaces;
using AstroManagerApi.Library.Models.Request;
using System.Security.Cryptography;

namespace AstroManagerApi.Library.Encryption;
public class RecoveryKeyGenerator : IRecoveryKeyGenerator
{
    private const int NumberOfKeys = 3;

    private static string GenerateRandomBase64Key()
    {
        byte[] keyBytes = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }

        return Convert.ToBase64String(keyBytes);
    }

    public RecoveryRequestModel GenerateRecoveryRequest()
    {
        var request = new RecoveryRequestModel();
        for (int i = 0; i < NumberOfKeys; i++)
        {
            string plainkey = GenerateRandomBase64Key();
            request.PlainRecoveryKeys.Add(plainkey);
        }

        foreach (var key in request.PlainRecoveryKeys)
        {
            request.Recovery.RecoveryKeys.Add(key);
        }

        return request;
    }
}
