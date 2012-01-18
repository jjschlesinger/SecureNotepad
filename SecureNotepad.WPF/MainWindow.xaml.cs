using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using SecureNotepad.Core.FileManagers;
using SecureNotepad.Core.UI;
using SecureNotepad.WPF.ViewModels;

namespace SecureNotepad.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _filter = "Secure text file|*.stf;|Plain text files|*.txt";
        private MainViewModel _main;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _main = (MainViewModel)DataContext;
            _main.UserSettings = new UserSettings();
            Locator.GetViewModel<SettingsViewModel>().UserSettings = _main.UserSettings;

            if (_main.UserSettings.FirstLaunch)
            {
                var settings = new SettingsPage();
                settings.ShowDialog();
                return;
            }

            Messenger.Default.Register<DialogResult>(this, "OpenFile", dlg => ShowFileDialog(new OpenFileDialog(), dlg));
            Messenger.Default.Register<DialogResult>(this, "SaveFile", dlg => ShowFileDialog(new SaveFileDialog(), dlg));
            Messenger.Default.Register<DialogResult>(this, "GetPassword", dlg => ShowPasswordDialog(dlg));
            Messenger.Default.Register<Boolean>(this, "ConfirmSave", b => ConfirmClose());

           

            ProcessCLI();
        }

        private void ProcessCLI()
        {
            if (App.CLIArgs.Length > 0)
            {
                var dr = new DialogResult
                {
                    FilePath = App.CLIArgs[0],
                    DialogType = FileDialogType.Open
                };

                if (dr.FilePath.EndsWith(".stf"))
                    dr.SelectedFileType = SecureFileType.Encrypted;
                else
                    dr.SelectedFileType = SecureFileType.ClearText;

                _main.ProcessDialog(dr);
            }
        }

        private void ShowFileDialog(FileDialog dlg, DialogResult dlgResult)
        {
            dlg.DefaultExt = ".stf";
            dlg.Filter = _filter;

            var result = dlg.ShowDialog();

            if (result == true)
            {
                if (dlg.FilterIndex == 1)
                    dlgResult.SelectedFileType = SecureFileType.Encrypted;
                else
                    dlgResult.SelectedFileType = SecureFileType.ClearText;

                dlgResult.FilePath = dlg.FileName;
                //dlgResult.DialogType = dlg.GetType() == typeof(OpenFileDialog) ? FileDialogType.Open : FileDialogType.Save;

                _main.ProcessDialog(dlgResult);
            }
        }

        private void ShowPasswordDialog(DialogResult dlgResult)
        {
            if (_main.UserSettings.AESKeyType == KeyType.Password)
            {
                if (dlgResult.DialogType == FileDialogType.Open)
                    dlgResult.Password = GetPassword("Enter password used to encrypt the file");
                else
                    dlgResult.Password = GetPassword("Enter password to encrypt the file", true);
            }
            else if (_main.UserSettings.AESKeyType == KeyType.KeyFile)
            {
                dlgResult.Password = GetPassword("Enter password used to protect key file (leave blank for unencrypted key)");
            }

            _main.ProcessDialog(dlgResult);
        }


        private string GetPassword(string messageText, bool isNewPassword = false)
        {
            var dlg = new PasswordPrompt(messageText, isNewPassword);
            dlg.ShowDialog();
            return dlg.Password;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConfirmClose();
        }

        private bool ConfirmClose()
        {
            var mbr = MessageBox.Show("Save opened file before closing?", "Confirm Save", MessageBoxButton.YesNoCancel);
            switch (mbr)
            {
                case MessageBoxResult.Yes:
                    _main.SaveCommand.Execute(null);
                    break;
                case MessageBoxResult.No:
                    _main.IsDirty = false;
                    _main.CloseCommand.Execute(null);
                    break;
                default:
                    return false;
            }

            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !ConfirmClose();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsPage();
            settings.ShowDialog();
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _main.SaveCommand.Execute(null);
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _main.OpenCommand.Execute(null);
        }

        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ConfirmClose();
        }
    }
}
