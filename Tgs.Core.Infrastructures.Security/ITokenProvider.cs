using System.Threading.Tasks;

namespace Tgs.Core.Infrastructures.Security;

public interface ITokenProvider
{
    ValueTask<string> EncryptAsync<T>(T data);

    ValueTask<T?> DecryptAsync<T>(string token);
}