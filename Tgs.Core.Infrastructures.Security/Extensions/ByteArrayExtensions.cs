using System;
using System.Text;

namespace Tgs.Core.Infrastructures.Security.Extensions
{
    internal static class ByteArrayExtensions
    {
        public static string ToBase64String(this byte[] byteArray) =>
            Convert.ToBase64String(byteArray);

        public static string ToHexString(this byte[] byteArray)
        {
            var sb = new StringBuilder();

            foreach (var t in byteArray)
            {
                sb.Append(t.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}