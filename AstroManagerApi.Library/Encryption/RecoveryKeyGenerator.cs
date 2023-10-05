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

    public HashSet<string> GenerateBase64Keys()
    {
        var keys = new HashSet<string>();

        while (keys.Count <= NumberOfKeys)
        {
            string key = GenerateRandomBase64Key();
            if (keys.Contains(key) is false)
            {
                keys.Add(key);
            }
        }

        return keys;
    }
}
