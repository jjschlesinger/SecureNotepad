using System;
using System.IO;
using System.Linq;
using System.Text;
using SecureNotepad.Core.CryptoExtensions;

namespace SecureNotepad.Core.FileManagers
{

    public class SecureTextFileManager : IFileManager
    {
        private readonly KeyType _keyType;
        private readonly string _aesKeyPath;
        private readonly string _rsaContainerPath;
        private readonly string _password;
        private readonly bool _useRsaContainer;
        private readonly string _passwordSalt;

        public SecureTextFileManager()
        {
        }


        public SecureTextFileManager(KeyType keyType, string aesKeyPath, bool useRsaContainer, string rsaContainerPath, string password, string passwordSalt)
        {
            _keyType = keyType;
            _password = password;
            _rsaContainerPath = rsaContainerPath;
            _useRsaContainer = useRsaContainer;
            _aesKeyPath = aesKeyPath;
            _passwordSalt = passwordSalt;
        }

        public string FilePath { get; set; }

        public string OpenFile()
        {
            var b = File.ReadAllBytes(FilePath);
            var k = GetKeyBytes();
            return Encoding.UTF8.GetString(b.Decrypt(k));
        }


        public void SaveFile(string contents)
        {
            var b = Encoding.UTF8.GetBytes(contents);
            
            byte[] k;
            k = GetKeyBytes();
            
            byte[] encData;
            encData = b.Encrypt(k);
            File.WriteAllBytes(FilePath, encData);
        }

        private byte[] GetKeyBytes()
        {
            byte[] k = null;
            var saltBytes = Convert.FromBase64String(_passwordSalt);
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
