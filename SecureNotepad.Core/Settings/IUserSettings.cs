using System;
using System.Linq;
using SecureNotepad.Core.FileManagers;
using SecureNotepad.Core.Net.OAuth;

namespace SecureNotepad.Core.Settings
{
    public interface IUserSettings
    {
        KeyType AESKeyType { get; set; }
        string AESKeyPath { get; set; }
        bool FirstLaunch { get; set; }
        string PasswordSalt { get; set; }
        OAuthToken Token { get; set; }
        void Save();
        void Reload();
    }
}
