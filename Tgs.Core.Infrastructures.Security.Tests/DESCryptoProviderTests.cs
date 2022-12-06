using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Tgs.Core.Infrastructures.Security.Tests;

[TestFixture]
public class DESCryptoProviderTests
{
    [Test]
    public async Task Can_Encrypt_String()
    {
        var sut = new DESCryptoProvider();

        var iv = "jh2hnM6K";
        var key = "jh2hnM6K";

        var original = "An apple a day keeps the doctor away";

        var actual = await sut.EncryptStringAsync(original, iv, key, CipherMode.CBC, PaddingMode.PKCS7, null)
            .ConfigureAwait(false);

        actual.Should().NotBeNull()
            .And.BeOfType<string>()
            .And.NotBeEquivalentTo(original);
    }

    [Test]
    public async Task Can_Decrypt_String()
    {
        var sut = new DESCryptoProvider();

        var iv = "jh2hnM6K";
        var key = "jh2hnM6K";
        var cipherMode = CipherMode.CBC;
        var paddingMode = PaddingMode.PKCS7;

        var original = "An apple a day keeps the doctor away";

        var encrypted = await sut.EncryptStringAsync(original, iv, key, cipherMode, paddingMode, null)
            .ConfigureAwait(false);

        var actual = await sut.DecryptStringAsync(encrypted, iv, key, cipherMode, paddingMode, null)
            .ConfigureAwait(false);

        actual.Should().NotBeNullOrEmpty()
            .And.BeOfType<string>()
            .And.Be(original);
    }
}