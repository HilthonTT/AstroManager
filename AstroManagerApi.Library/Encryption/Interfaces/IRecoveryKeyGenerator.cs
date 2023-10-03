using AstroManagerApi.Library.Models.Request;

namespace AstroManagerApi.Library.Encryption.Interfaces;
public interface IRecoveryKeyGenerator
{
    HashSet<string> GenerateBase64Keys();
}