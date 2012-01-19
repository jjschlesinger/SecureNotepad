using System;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using SecureNotepad.Core.CryptoExtensions;
using SecureNotepad.Core.Settings;

namespace SecureNotepad.WPF.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private bool? _useFileKey;

        /// <summary>
        /// Sets and gets the UseFileKey property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool? UseFileKey
        {
            get
            {
                return _useFileKey;
            }

            set
            {
                if (_useFileKey == value)
                {
                    return;
                }

                _useFileKey = value;
                if (_userSettings != null && _useFileKey.HasValue && _useFileKey.Value)
                    _userSettings.AESKeyType = Core.FileManagers.KeyType.KeyFile;

                RaisePropertyChanged(() => UseFileKey);
            }
        }

        private bool? _usePasswordAsKey;

        /// <summary>
        /// Sets and gets the UsePasswordAsKey property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool? UsePasswordAsKey
        {
            get
            {
                return _usePasswordAsKey;
            }

            set
            {
                if (_usePasswordAsKey == value)
                {
                    return;
                }

                _usePasswordAsKey = value;
                if (_userSettings != null && _usePasswordAsKey.HasValue && _usePasswordAsKey.Value)
                    _userSettings.AESKeyType = Core.FileManagers.KeyType.Password;

                RaisePropertyChanged(() => UsePasswordAsKey);
            }
        }

        /// <summary>
        /// Sets and gets the AESKeyPath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AESKeyPath
        {
            get
            {
                return _userSettings.AESKeyPath;
            }

            set
            {
                if (_userSettings.AESKeyPath == value)
                {
                    return;
                }

                _userSettings.AESKeyPath = value;
                RaisePropertyChanged(() => AESKeyPath);
            }
        }

        private IUserSettings _userSettings;

        /// <summary>
        /// Sets and gets the UserSettings property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IUserSettings UserSettings
        {
            get
            {
                return _userSettings;
            }

            set
            {
                if (_userSettings == value)
                {
                    return;
                }

                _userSettings = value;
                RaisePropertyChanged(() => UserSettings);
                SetKeyProperties();
            }
        }

        public SettingsViewModel()
        {
            SaveCommand = new RelayCommand<String>(keyPass => SaveSettings(keyPass));
        }

        public RelayCommand<String> SaveCommand { get; private set; }

        private void SetKeyProperties()
        {
            if (_userSettings == null)
                return;

            switch (_userSettings.AESKeyType)
            {
                case SecureNotepad.Core.FileManagers.KeyType.Password:
                    _usePasswordAsKey = true;
                    _useFileKey = false;
                    break;
                case SecureNotepad.Core.FileManagers.KeyType.KeyFile:
                    _usePasswordAsKey = false;
                    _useFileKey = true;
                    break;
            }
        }

        private void SaveSettings(string keyPass)
        {
            if (keyPass == null && !File.Exists(AESKeyPath))
            {
                MessengerInstance.Send<Boolean>(true, "GetPassword");
                return;
            }
            var saltBytes = new byte[16];

            if (String.IsNullOrEmpty(_userSettings.PasswordSalt))
                saltBytes = RNGExtensions.GetRandomBytes(16);
            else
                saltBytes = Convert.FromBase64String(_userSettings.PasswordSalt);

            if (!File.Exists(AESKeyPath))
            {
                var k = new byte[32];

                if (UseFileKey.HasValue && UseFileKey.Value)
                {
                    k = RNGExtensions.GetRandomBytes(32);
                    if (!String.IsNullOrEmpty(keyPass))
                        k = k.Encrypt(keyPass.GetKeyFromPassphrase(32, saltBytes));

                    File.WriteAllBytes(AESKeyPath, k);
                }


            }

            _userSettings.AESKeyPath = AESKeyPath;
            _userSettings.FirstLaunch = false;
            _userSettings.PasswordSalt = Convert.ToBase64String(saltBytes);
            _userSettings.Save();

            MessengerInstance.Send<Boolean>(true, "SaveComplete");
        }

        public override void Cleanup()
        {
            _userSettings.Reload();
            SetKeyProperties();
            
            base.Cleanup();

            Locator.UnregisterVM<SettingsViewModel>();
        }
    }
}