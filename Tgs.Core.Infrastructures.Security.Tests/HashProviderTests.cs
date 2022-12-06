using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Tgs.Core.Infrastructures.Security.Enums;

namespace Tgs.Core.Infrastructures.Security.Tests;

public class HashProviderTests
{
    [Test]
    public void Can_Hash_String()
    {
        var str = "This is a book";
        var algorithm = HashAlgorithmKind.Md5;
        var kind = HashStringKind.Base64;
        var encoding = Encoding.ASCII;

        var sut = new HashProvider();
        var hash = sut.Hash(str, algorithm, kind, encoding);

        hash.Should().NotBeNullOrWhiteSpace();
        hash.Should().NotBe(str);
    }

    [Test]
    public void Can_Hash_Array_With_Default_Salt_And_Algorithm()
    {
        var str = "This is a book";
        var array = Encoding.UTF8.GetBytes(str);

        var sut = new HashProvider();
        var hash = sut.Hash(array);

        hash.Length.Should().NotBe(0);
    }

    [Test]
    public void Hashing_Same_Arrays_With_Settings_Should_Have_Same_Hash()
    {
        var encoding = Encoding.UTF8;

        var str = "This is a book";
        var array = encoding.GetBytes(str);

        var saltStr = "I_am_salt";
        var salt = encoding.GetBytes(saltStr);
        var algorithm = HashAlgorithmKind.Sha384;

        var sut = new HashProvider();
        var hash1 = sut.Hash(array, salt, algorithm);
        var hash2 = sut.Hash(array, salt, algorithm);

        hash1.Should().BeEquivalentTo(hash2);
    }

    [Test]
    public void Hashing_Different_Arrays_Should_Have_Different_Hash()
    {
        var encoding = Encoding.UTF8;

        var str1 = "This is a book";
        var array1 = encoding.GetBytes(str1);

        var str2 = "A apple a day keeps the doctor away";
        var array2 = encoding.GetBytes(str2);

        var saltStr = "I_am_salt";
        var salt = encoding.GetBytes(saltStr);
        var algorithm = HashAlgorithmKind.Sha384;

        var sut = new HashProvider();
        var hash1 = sut.Hash(array1, salt, algorithm);
        var hash2 = sut.Hash(array2, salt, algorithm);

        hash1.Should().NotBeEquivalentTo(hash2);
    }

    [Test]
    public void Hashing_Same_String_With_Settings_Should_Have_Same_Hash()
    {
        var str = "This is a book";

        var encoding = Encoding.ASCII;
        var algorithm = HashAlgorithmKind.Sha512;
        var kind = HashStringKind.Hex;

        var sut = new HashProvider();
        var hash1 = sut.Hash(str, algorithm, kind, encoding);
        var hash2 = sut.Hash(str, algorithm, kind, encoding);

        hash1.Should().BeEquivalentTo(hash2);
    }

    [Test]
    public void Hashing_Different_String_Should_Have_Different_Hash()
    {
        var str1 = "This is a book";
        var str2 = "A apple a day keeps the doctor away";

        var encoding = Encoding.ASCII;
        var algorithm = HashAlgorithmKind.Sha512;
        var kind = HashStringKind.Hex;

        var sut = new HashProvider();
        var hash1 = sut.Hash(str1, algorithm, kind, encoding);
        var hash2 = sut.Hash(str2, algorithm, kind, encoding);

        hash1.Should().NotBeEquivalentTo(hash2);
    }
}