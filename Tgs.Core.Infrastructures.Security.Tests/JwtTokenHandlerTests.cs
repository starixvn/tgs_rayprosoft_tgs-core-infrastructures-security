using System;
using System.Linq;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace Tgs.Core.Infrastructures.Security.Tests
{
    [TestFixture]
    public class JwtTokenHandlerTests
    {
        [Test]
        public void Can_Generate_Token()
        {
            var hashProvider = new HashProvider();
            var passwordHandler = new PasswordHandler(hashProvider);
            var sut = new JwtTokenHandler();

            var identity = new ClaimsIdentity(
                new Claim[]
                {
                    new("sub", "sub"),
                    new("jwt", "jwt")
                });

            var securityKey = passwordHandler.GenerateRandomSalt(30);
            var issuer = "issuer";
            var expires = DateTime.Now.AddDays(1);

            var actual = sut.GenerateToken(identity, securityKey, issuer, expires);

            actual.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void Can_Read_Token()
        {
            var hashProvider = new HashProvider();
            var passwordHandler = new PasswordHandler(hashProvider);
            var sut = new JwtTokenHandler();

            var claims = new Claim[]
            {
                new("sub", "sub"),
                new("jwt", "jwt")
            };

            var identity = new ClaimsIdentity(claims);

            var securityKey = passwordHandler.GenerateRandomSalt(30);
            var issuer = "issuer";
            var expires = DateTime.Now.AddDays(1);

            var tokenString = sut.GenerateToken(identity, securityKey, issuer, expires);

            var token = sut.ReadToken(tokenString);

            token.Should().NotBeNull();

            foreach (var claim in claims)
            {
                var contains = token.Claims.Any(c => c.Type == claim.Type);
                contains.Should().BeTrue();
            }
        }

        [Test]
        public void Validate_Correct_Token_Should_Pass()
        {
            var hashProvider = new HashProvider();
            var passwordHandler = new PasswordHandler(hashProvider);
            var sut = new JwtTokenHandler();

            var claims = new Claim[]
            {
                new("sub", "sub"),
                new("jwt", "jwt")
            };

            var identity = new ClaimsIdentity(claims);

            var securityKey = passwordHandler.GenerateRandomSalt(30);
            var issuer = "issuer";
            var expires = DateTime.Now.AddDays(1);

            var tokenString = sut.GenerateToken(identity, securityKey, issuer, expires);

            var token = sut.ReadToken(tokenString);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var (isValid, claimPrinciple, securityToken) =
                sut.ValidateToken(tokenString, tokenValidationParameters, null, null);

            isValid.Should().BeTrue();
        }
    }
}