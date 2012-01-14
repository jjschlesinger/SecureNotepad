using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoExtensions;
using System.IO;

namespace SecureNotepad
{

    class SecureTextFileManager : IFileManager
    {
        private KeyType _keyType;
        private string _aesKeyPath;
        private string _rsaContainerPath;
        private string _password;
        private bool _useRsaContainer;

        public SecureTextFileManager()
        {
        }


        public SecureTextFileManager(KeyType keyType, string aesKeyPath, bool useRsaContainer, string rsaContainerPath, string password)
        {
            _keyType = keyType;
            _password = password;
            _rsaContainerPath = rsaContainerPath;
            _useRsaContainer = useRsaContainer;
            _aesKeyPath = aesKeyPath;
        }

        public string FilePath
        {
            get;
            set;
        }

        public string OpenFile()
        {
            var b = File.ReadAllBytes(FilePath);
            var k = GetKeyBytes();

            return Encoding.UTF8.GetString(b.Decrypt(k));
        }

        
        public void SaveFile(string contents)
        {
            var b = Encoding.UTF8.GetBytes(contents);
            var k = GetKeyBytes();
            File.WriteAllBytes(FilePath, b.Encrypt(k));
        }

        private byte[] GetKeyBytes()
        {
            byte[] k = null;
            var saltBytes = Convert.FromBase64String(User.Default.PasswordSalt);
            switch (_keyType)
            {
                case KeyType.Password:
                    k = _password.GetKeyFromPassphrase(32, saltBytes);
                    break;
                case KeyType.KeyFile:
                    k = File.ReadAllBytes(_aesKeyPath);
                    if (!String.IsNullOrEmpty(_password))
                    {
                        //if password is set, decrypt key using password
                        k = k.Decrypt(_password.GetKeyFromPassphrase(32, saltBytes));
                    }
                    break;
                case KeyType.RsaEncryptedKeyFile:
                    k = File.ReadAllBytes(_aesKeyPath);
                    if (_useRsaContainer)
                        k = k.Decrypt(_rsaContainerPath.ExportRSAKey());
                    else
                        k = k.Decrypt(_rsaContainerPath.ExportRSAKeyFromXml());
                    break;
            }

            if (k == null)
                throw new Exception("Unable to get key file");

            return k;
        }
    }
}
