using System.Text.Json;
using System.Threading.Tasks;
using Tgs.Core.Infrastructures.Security.Enums;

namespace Tgs.Core.Infrastructures.Security
{
    public interface ITokenProvider
    {
        Task<string> EncryptAsync<T>(T data);

        Task<T> DecryptAsync<T>(string token);
    }

    public sealed class TokenProvider : ITokenProvider
    {
        private static readonly string _iv = "37F8BF8F8EA48FD2";
        private static readonly string _key = "D42F6412E48DD386";
        private readonly ISymmetricCryptoProvider _cryptoProvider;

        private readonly JsonSerializerOptions _options;

        public TokenProvider(
            ISymmetricCryptoProviderFactory cryptoProviderFactory)
        {
            _cryptoProvider = cryptoProviderFactory.GetProvider(SymmetricAlgorithmKind.AESBase62);

            _options = new JsonSerializerOptions
            {
                WriteIndented = false
            };
        }

        public async Task<T> DecryptAsync<T>(string token)
        {
            var value = await _cryptoProvider.DecryptStringAsync(token, _iv, _key);

            return JsonSerializer.Deserialize<T>(value, _options);
        }

        public Task<string> EncryptAsync<T>(T data)
        {
            var value = JsonSerializer.Serialize(data, _options);

            return _cryptoProvider.EncryptStringAsync(value, _iv, _key);
        }
    }
}