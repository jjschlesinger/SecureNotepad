using System;
using System.Linq;
using SecureNotepad.Core.FileManagers;

namespace SecureNotepad.Core.Settings
{
    public interface IUserSettings
    {
        KeyType AESKeyType { get; set; }
        string AESKeyPath { get; set; }
        bool FirstLaunch { get; set; }
        string PasswordSalt { get; set; }
        void Save();
    }
}
