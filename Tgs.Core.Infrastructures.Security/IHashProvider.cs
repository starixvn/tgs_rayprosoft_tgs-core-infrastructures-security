using System.Text;
using Tgs.Core.Infrastructures.Security.Enums;

namespace Tgs.Core.Infrastructures.Security
{
    public interface IHashProvider
    {
        string Hash(
            string plainText, HashAlgorithmKind algorithm = HashAlgorithmKind.Sha512,
            HashStringKind kind = HashStringKind.Hex, Encoding? encoding = null);

        byte[] Hash(byte[] data, byte[] salt, HashAlgorithmKind algorithm = HashAlgorithmKind.Sha512);

        byte[] Hash(byte[] data);
    }
}