using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Tgs.Core.Infrastructures.Security.Tests;

[TestFixture]
public class TokenProviderTests
{
    [Test]
    public async Task Can_Encrypt_Token()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<AESBase62CryptoProvider>();

        await using var serviceProvider = services.BuildServiceProvider();

        var cryptoProviderFactory = new SymmetricCryptoProviderFactory(serviceProvider);
        var tokenProvider = new TokenProvider(cryptoProviderFactory);

        var o = new User("Tom", 40);
        var actual = await tokenProvider.EncryptAsync(o).ConfigureAwait(false);

        actual.Should().NotBeNull()
            .And.BeOfType<string>()
            .And
            .Subject.Length.Should().BeGreaterThan(0);
    }

    [Test]
    public async Task Can_Decrypt_Token()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<AESBase62CryptoProvider>();

        await using var serviceProvider = services.BuildServiceProvider();

        var cryptoProviderFactory = new SymmetricCryptoProviderFactory(serviceProvider);
        var tokenProvider = new TokenProvider(cryptoProviderFactory);

        var user = new User("Tom", 20);
        var token = await tokenProvider.EncryptAsync(user).ConfigureAwait(false);

        var actual = await tokenProvider.DecryptAsync<User>(token).ConfigureAwait(false);

        actual.Should().NotBeNull()
            .And.Be(user);
    }

    private record User(string Name, int Age);
}