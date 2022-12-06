using System.Security.Cryptography;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Tgs.Core.Infrastructures.Security.Tests;

[TestFixture]
public class AESBase62CryptoProviderTests
{
    [Test]
    public async Task Can_Encrypt_String()
    {
        var sut = new AESBase62CryptoProvider();

        var iv = "756edb2fc85fe908";
        var key = "4f09f467e6a8fb9f";

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
        var sut = new AESBase62CryptoProvider();

        var iv = "756edb2fc85fe908";
        var key = "4f09f467e6a8fb9f";
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