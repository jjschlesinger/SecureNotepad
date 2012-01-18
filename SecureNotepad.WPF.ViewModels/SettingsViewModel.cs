using GalaSoft.MvvmLight;
using SecureNotepad.Core.Settings;

namespace SecureNotepad.WPF.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the SettingsViewModel class.
        /// </summary>
        public SettingsViewModel()
        {
        }

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
                RaisePropertyChanged(() => UsePasswordAsKey);
            }
        }

        /// <summary>
        /// The <see cref="AESKeyPath" /> property's name.
        /// </summary>
        public const string AESKeyPathPropertyName = "AESKeyPath";

        private string _aesKeyPath;

        /// <summary>
        /// Sets and gets the AESKeyPath property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AESKeyPath
        {
            get
            {
                return _aesKeyPath;
            }

            set
            {
                if (_aesKeyPath == value)
                {
                    return;
                }

                _aesKeyPath = value;
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
            }
        }
    }
}