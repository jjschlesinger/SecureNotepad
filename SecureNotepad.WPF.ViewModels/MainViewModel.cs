using System;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SecureNotepad.Core.FileManagers;
using SecureNotepad.Core.Settings;
using SecureNotepad.Core.UI;
using System.Text.RegularExpressions;

namespace SecureNotepad.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IFileManager _activeFileMansager;
        private string _fileContents = null;

        public bool IsDirty { get; set; }

        

        /// <summary>
        /// Sets and gets the FileContents property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string FileContents
        {
            get
            {
                return _fileContents;
            }

            set
            {
                if (_fileContents == value)
                {
                    return;
                }

                _fileContents = value;
                IsDirty = true;

                RaisePropertyChanged(() => FileContents);
            }
        }

        private int _positionInContents = 0;

        /// <summary>
        /// Sets and gets the PositionInContents property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int PositionInContents
        {
            get
            {
                return _positionInContents;
            }

            set
            {
                if (_positionInContents == value)
                {
                    return;
                }

                _positionInContents = value;
                RaisePropertyChanged(() => PositionInContents);
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

        public RelayCommand OpenCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        public RelayCommand SettingsCommand { get; private set; }
        public RelayCommand<String> FindCommand { get; private set; }

        public MainViewModel()
        {
            OpenCommand = new RelayCommand(() => SendOpenFileMessage());
            SaveCommand = new RelayCommand(() => SendSaveFileMessage());
            CloseCommand = new RelayCommand(() => SendCloseFileMessage());
            SettingsCommand = new RelayCommand(() => SendSettingsMessage());
            FindCommand = new RelayCommand<String>(k => FindInContents(k));
        }

        private void FindInContents(string key)
        {
            var regex = new Regex(key, RegexOptions.IgnoreCase);
            var matches = regex.Matches(FileContents, PositionInContents);
            if(matches.Count > 0)
                MessengerInstance.Send<String>(matches[0].Value, "FoundMatch");
            else
                MessengerInstance.Send<String>(String.Empty, "FoundMatch");
        }

        private void SendSettingsMessage()
        {
            MessengerInstance.Send<bool>(true, "ShowSettings");
        }

        private void SendCloseFileMessage()
        {
            if (IsDirty)
            {
                MessengerInstance.Send<bool>(true, "ConfirmSave");
                return;
            }

            _activeFileMansager = null;
            FileContents = String.Empty;
            IsDirty = false;
        }

        private void SendOpenFileMessage()
        {
            var dlg = new DialogResult() { DialogType = FileDialogType.Open };
            MessengerInstance.Send<DialogResult>(dlg, "OpenFile");
        }

        private void SendSaveFileMessage()
        {
            var dlg = new DialogResult() { DialogType = FileDialogType.Save };
            MessengerInstance.Send<DialogResult>(dlg, "SaveFile");
        }

        public void ProcessDialog(DialogResult dlgResult)
        {
            if (dlgResult.Password == null)
            {
                MessengerInstance.Send<DialogResult>(dlgResult, "GetPassword");
                return;
            }
   
            switch (dlgResult.SelectedFileType)
            {
                case SecureFileType.Encrypted:
                    _activeFileMansager = new SecureTextFileManager(_userSettings.AESKeyType, _userSettings.AESKeyPath, false, null, dlgResult.Password, _userSettings.PasswordSalt);
                    break;
                case SecureFileType.ClearText:
                    _activeFileMansager = new PlainTextFileManager();
                    break;
            }

            _activeFileMansager.FilePath = dlgResult.FilePath;


            if (dlgResult.DialogType == FileDialogType.Open)
            {
                _activeFileMansager.LoadFile();
                _fileContents =_activeFileMansager.FileContents;
            }
            else
            {
                _activeFileMansager.FileContents = _fileContents;
                _activeFileMansager.SaveFile();
                IsDirty = false;
            }

            RaisePropertyChanged(() => FileContents);
        }

    }
}
