namespace AstroManagerClient.Library.Encryption;
public static class PasswordGenerator
{
    private const string ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+<>~-";

    public static string GenerateRandomPassword(int length = 20)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);

        var password = new char[length];

        Parallel.For(0, length, (i) =>
        {
            int index = bytes[i] % ValidChars.Length;
            password[i] = ValidChars[index];
        });

        return new string(password);
    }
}
