using System.Text.Json;
using System.Threading.Tasks;
using Tgs.Core.Infrastructures.Security.Enums;

namespace Tgs.Core.Infrastructures.Security;

public sealed class TokenProvider : ITokenProvider
{
    private static readonly string _Iv = "37F8BF8F8EA48FD2";
    private static readonly string _Key = "D42F6412E48DD386";
    private readonly ISymmetricCryptoProvider _cryptoProvider;

    private readonly JsonSerializerOptions? _options;

    public TokenProvider(
        ISymmetricCryptoProviderFactory cryptoProviderFactory)
    {
        _cryptoProvider = cryptoProviderFactory.GetProvider(SymmetricAlgorithmKind.AESBase62);

        _options = new JsonSerializerOptions
        {
            WriteIndented = false
        };
    }

    public async ValueTask<T?> DecryptAsync<T>(string token)
    {
        var value = await _cryptoProvider.DecryptStringAsync(token, _Iv, _Key)
            .ConfigureAwait(false);

        return JsonSerializer.Deserialize<T>(value, _options);
    }

    public ValueTask<string> EncryptAsync<T>(T data)
    {
        var value = JsonSerializer.Serialize(data, _options);

        return _cryptoProvider.EncryptStringAsync(value, _Iv, _Key);
    }
}