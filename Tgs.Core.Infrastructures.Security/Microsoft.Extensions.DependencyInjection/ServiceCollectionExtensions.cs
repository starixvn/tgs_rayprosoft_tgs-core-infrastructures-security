// ReSharper disable CheckNamespace

using Tgs.Core.Infrastructures.Security;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Dependency Injection of Infrastructures.Security
        /// </summary>
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddSingleton<IHashProvider, HashProvider>();
            services.AddSingleton<IPasswordHandler, PasswordHandler>();

            services.AddSingleton<DESCryptoProvider>();
            services.AddSingleton<AESCryptoProvider>();
            services.AddSingleton<AESBase62CryptoProvider>();

            services.AddSingleton<ISymmetricCryptoProviderFactory, SymmetricCryptoProviderFactory>();
            services.AddSingleton<ITokenProvider, TokenProvider>();

            services.AddSingleton<IJwtTokenHandler, JwtTokenHandler>();

            return services;
        }
    }
}