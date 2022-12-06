using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Tgs.Core.Infrastructures.Security;

public interface IJwtTokenHandler
{
    string GenerateToken(ClaimsIdentity identity, byte[] securityKey, string issuer, DateTime? expires);

    JwtSecurityToken ReadToken(string token);

    /// <summary>
    /// Validate Jwt Token
    /// </summary>
    /// <remarks>
    /// It will use onError implementation by default when onExpired is null,
    /// if onError is null, means all errors will be passed.
    /// </remarks>
    (bool IsValid, ClaimsPrincipal? ClaimsPrincipal, SecurityToken? ValidatedToken) ValidateToken(
        string token,
        TokenValidationParameters parameters,
        Action<Exception>? onError,
        Action<SecurityTokenExpiredException>? onExpired = null);
}