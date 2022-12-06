using Tgs.Core.Infrastructures.Security.Enums;

namespace Tgs.Core.Infrastructures.Security;

public interface ISymmetricCryptoProviderFactory
{
    ISymmetricCryptoProvider GetProvider(SymmetricAlgorithmKind algorithm);
}