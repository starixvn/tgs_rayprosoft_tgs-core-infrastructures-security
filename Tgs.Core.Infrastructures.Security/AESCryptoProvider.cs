// ReSharper disable InconsistentNaming

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.Core.Infrastructures.Security
{
    internal sealed class AESCryptoProvider : ISymmetricCryptoProvider
    {
        public async Task<string> DecryptStringAsync(
            string encrypted, string iv, string key,
            CipherMode mode,
            PaddingMode padding, Encoding encoding)
        {
            if (string.IsNullOrEmpty(encrypted))
            {
                return encrypted;
            }

            ValidateIVAndKey(iv, key);

            encoding ??= Encoding.UTF8;

            using var aes = new AesCryptoServiceProvider
            {
                IV = encoding.GetBytes(iv),
                Key = encoding.GetBytes(key),
                Mode = mode,
                Padding = padding
            };

            using var ct = aes.CreateDecryptor();
            await using var ms = new MemoryStream();
            await using var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            var bytes = Convert.FromBase64String(encrypted);
            await cs.WriteAsync(bytes.AsMemory(0, bytes.Length));
            await cs.FlushFinalBlockAsync();

            return encoding.GetString(ms.ToArray());
        }

        public async Task<string> EncryptStringAsync(
            string original, string iv, string key,
            CipherMode mode, PaddingMode padding, Encoding encoding)
        {
            if (string.IsNullOrEmpty(original))
            {
                return original;
            }

            ValidateIVAndKey(iv, key);

            encoding ??= Encoding.UTF8;

            using var aes = new AesCryptoServiceProvider
            {
                IV = encoding.GetBytes(iv),
                Key = encoding.GetBytes(key),
                Mode = mode,
                Padding = padding
            };

            using var ct = aes.CreateEncryptor();
            await using var ms = new MemoryStream();
            await using var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);

            var bytes = encoding.GetBytes(original);
            await cs.WriteAsync(bytes.AsMemory(0, bytes.Length));
            await cs.FlushFinalBlockAsync();

            return Convert.ToBase64String(ms.ToArray());
        }

        private static void ValidateIVAndKey(string iv, string key)
        {
            if (string.IsNullOrEmpty(iv) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("IV and Key for AES should not be null or empty");
            }
        }
    }
}