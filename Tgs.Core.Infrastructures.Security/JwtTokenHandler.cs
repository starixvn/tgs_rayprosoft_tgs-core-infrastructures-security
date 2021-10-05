using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Tgs.Core.Infrastructures.Security
{
    public sealed class JwtTokenHandler : IJwtTokenHandler
    {
        private readonly JwtSecurityTokenHandler _handler = new();

        public string GenerateToken(
            ClaimsIdentity identity, byte[] securityKey, string issuer,
            DateTime? expires)
        {
            var key = new SymmetricSecurityKey(securityKey);
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Issuer = issuer,
                Expires = expires,
                SigningCredentials = credential
            };

            var securityToken = _handler.CreateToken(descriptor);

            return _handler.WriteToken(securityToken);
        }

        public JwtSecurityToken ReadToken(string token) =>
            _handler.ReadJwtToken(token);

        public (bool, ClaimsPrincipal, SecurityToken) ValidateToken(
            string token,
            TokenValidationParameters parameters, Action<Exception> onError,
            Action<SecurityTokenExpiredException> onExpired)
        {
            try
            {
                var claimsPrincipal = _handler.ValidateToken(token, parameters, out var validatedToken);

                return (true, claimsPrincipal, validatedToken);
            }
            catch (SecurityTokenExpiredException e)
            {
                onExpired ??= onError;
                onExpired?.Invoke(e);
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
            }

            return (false, null, null);
        }
    }
}