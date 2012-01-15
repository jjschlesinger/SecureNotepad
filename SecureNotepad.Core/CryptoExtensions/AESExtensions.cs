using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SecureNotepad.Core.CryptoExtensions
{
    public static class AESExtensions
    {

        public static byte[] WriteCryptoStream(this byte[] data, byte[] key, CryptoType cryptoType)
        {
            using (var aes = new RijndaelManaged())
            {
                aes.Key = key;
                aes.IV = new byte[16];
                if (cryptoType == CryptoType.Encrypt)
                    aes.IV = RNGExtensions.GetRandomBytes(16);
                else
                {
                    aes.IV = ReadIV(data);
                }
                
                ICryptoTransform crypto;

                if (cryptoType == CryptoType.Decrypt)
                    crypto = aes.CreateDecryptor(aes.Key, aes.IV);
                else
                    crypto = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    if (cryptoType == CryptoType.Encrypt)
                        ms.Write(aes.IV, 0, aes.IV.Length);
                    
                    using (CryptoStream csCrypt = new CryptoStream(ms, crypto, CryptoStreamMode.Write))
                    {
                        if (cryptoType == CryptoType.Encrypt)
                        {
                            csCrypt.Write(data, 0, data.Length);
                        }
                        else
                        {
                            csCrypt.Write(data, 16, data.Length - 16);
                        }

                        csCrypt.FlushFinalBlock();
                        ms.Position = 0;
                        var b = new byte[ms.Length];
                        ms.Read(b, 0, b.Length);
                        return b;
                    }
                }
            }
        }

        public static byte[] GetKeyFromPassphrase(this string passphrase, int keySize, byte[] salt)
        {
            var pdb = new Rfc2898DeriveBytes(passphrase, salt);
            return pdb.GetBytes(keySize);
        }

        public static byte[] Encrypt(this byte[] data, byte[] key)
        {
            return data.WriteCryptoStream(key, CryptoType.Encrypt);
            
        }

        public static byte[] Decrypt(this byte[] data, byte[] key)
        {
            return data.WriteCryptoStream(key, CryptoType.Decrypt);
        }

        public static byte[] ReadIV(byte[] data)
        {
            var iv = new byte[16];

            for (var i = 0; i < 16; i++)
            {
               iv[i] = data[i];
            }

            return iv;
        }
    }
}
