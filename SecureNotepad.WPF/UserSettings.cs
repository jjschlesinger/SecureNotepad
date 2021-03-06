﻿using System;
using System.Linq;
using SecureNotepad.Core.FileManagers;
using SecureNotepad.Core.Settings;

namespace SecureNotepad.WPF
{
    class UserSettings : IUserSettings
    {
        public UserSettings()
        {
            User.Default.Reload();
        }

        public KeyType AESKeyType
        {
            get
            {
                return (KeyType)User.Default.KeyType;
            }
            set
            {
                User.Default.KeyType = Convert.ToByte(value);
            }
        }

        public string AESKeyPath
        {
            get
            {
                return User.Default.AESKeyPath;
            }
            set
            {
                User.Default.AESKeyPath = value;
            }
        }

        public bool FirstLaunch
        {
            get
            {
                return User.Default.FirstLaunch;
            }
            set
            {
                User.Default.FirstLaunch = value;
            }
        }

        public string PasswordSalt
        {
            get
            {
                return User.Default.PasswordSalt;
            }
            set
            {
                User.Default.PasswordSalt = value;
            }
        }
        
        public void Save()
        {
            User.Default.Save();
        }

        public void Reload()
        {
            User.Default.Reload();
        }
    }
}
