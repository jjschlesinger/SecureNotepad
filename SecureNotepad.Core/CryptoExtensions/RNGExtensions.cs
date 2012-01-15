using System;
using System.Linq;
using System.Security.Cryptography;

namespace SecureNotepad.Core.CryptoExtensions
{
    public static class RNGExtensions
    {
        public static byte[] GetRandomBytes(int numBytes)
        {
            var rng = new RNGCryptoServiceProvider();
            var b = new byte[numBytes];
            rng.GetBytes(b);
            return b;
        }
    }
}
