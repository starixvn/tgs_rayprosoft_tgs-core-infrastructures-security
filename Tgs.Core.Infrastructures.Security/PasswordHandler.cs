using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tgs.Core.Infrastructures.Security;

public sealed class PasswordHandler : IPasswordHandler
{
    private readonly IHashProvider _hashProvider;
    private readonly RandomNumberGenerator _randomNumberGenerator;

    public PasswordHandler(IHashProvider hashProvider)
    {
        _hashProvider = hashProvider;
        _randomNumberGenerator = RandomNumberGenerator.Create();
    }

    public (byte[] Salt, byte[] HashedPassword) GenerateCredential(string password)
    {
        var salt = GenerateRandomSalt(32);

        var hashedPassword = _hashProvider.Hash(
            Encoding.UTF8.GetBytes(password),
            _hashProvider.Hash(salt));

        return (salt, hashedPassword);
    }

    public bool Verify(byte[] existedPassword, byte[] existedSalt, string password)
    {
        var salt = _hashProvider.Hash(existedSalt);

        var hashedPassword = _hashProvider.Hash(
            Encoding.UTF8.GetBytes(password),
            salt);

        return existedPassword.SequenceEqual(hashedPassword);
    }

    public byte[] GenerateRandomSalt(ushort length)
    {
        if (length == 0)
        {
            throw new ArgumentException("Length of random bytes generated cannot be 0");
        }

        var bytes = new byte[length];
        _randomNumberGenerator.GetNonZeroBytes(bytes);

        return bytes;
    }
}