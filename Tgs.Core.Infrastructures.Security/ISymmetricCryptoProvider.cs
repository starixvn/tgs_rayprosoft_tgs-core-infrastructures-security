using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.Core.Infrastructures.Security;

public interface ISymmetricCryptoProvider
{
    ValueTask<string> EncryptStringAsync(
        string original,
        string iv,
        string key,
        CipherMode mode = CipherMode.CBC,
        PaddingMode padding = PaddingMode.PKCS7,
        Encoding? encoding = null);

    ValueTask<string> DecryptStringAsync(
        string encrypted,
        string iv,
        string key,
        CipherMode mode = CipherMode.CBC,
        PaddingMode padding = PaddingMode.PKCS7,
        Encoding? encoding = null);
}