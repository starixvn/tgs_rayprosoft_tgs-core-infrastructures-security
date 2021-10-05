using System.Text;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Tgs.Core.Infrastructures.Security.Tests
{
    [TestFixture]
    public class PasswordHandlerTests
    {
        [Test]
        public void Can_Generate_Credential()
        {
            var hashProvider = Substitute.For<IHashProvider>();
            var sut = new PasswordHandler(hashProvider);

            var password = "P@ssw0rd";

            var hashed = Encoding.UTF8.GetBytes(password);

            hashProvider.Hash(Arg.Any<byte[]>(), Arg.Any<byte[]>())
                .Returns(hashed);

            var (salt, hashedPassword) = sut.GenerateCredential(password);

            salt.Should().NotBeNull();
            salt.Length.Should().BeGreaterThan(0);
            hashedPassword.Should().NotBeNull();
            hashedPassword.Length.Should().BeGreaterThan(0);
        }

        [Test]
        public void Can_Generate_Random_Slat()
        {
            var hashProvider = Substitute.For<IHashProvider>();
            var sut = new PasswordHandler(hashProvider);

            ushort length = 30;
            var salt = sut.GenerateRandomSalt(length);

            salt.Should().NotBeNull();
            salt.Length.Should().BeGreaterThan(0);
        }

        [Test]
        public void Input_Correct_Password_Should_Pass_Verify()
        {
            var hashProvider = new HashProvider();
            var sut = new PasswordHandler(hashProvider);

            var password = "TgsP@ssw0rd";
            var (salt, hashedPassword) = sut.GenerateCredential(password);

            var actual = sut.Verify(hashedPassword, salt, password);

            actual.Should().BeTrue();
        }

        [Test]
        public void Input_Wrong_Password_Should_Not_Pass_Verify()
        {
            var hashProvider = new HashProvider();
            var sut = new PasswordHandler(hashProvider);

            var password = "TgsP@ssw0rd";
            var (salt, hashedPassword) = sut.GenerateCredential(password);

            var wrongPassword = "WrongPassword";
            var actual = sut.Verify(hashedPassword, salt, wrongPassword);

            actual.Should().BeFalse();
        }
    }
}