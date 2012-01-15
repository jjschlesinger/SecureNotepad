using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SecureNotepad.Core.FileManagers;

namespace SecureNotepad.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _filter = "Secure text file|*.stf;|Plain text files|*.txt";
        private IFileManager _fileMgr;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(User.Default.FirstLaunch)
            {
                var settings = new SettingsPage();
                settings.ShowDialog();
                return;
            }

            if (App.CLIArgs.Length > 0)
                OpenFile(App.CLIArgs[0]);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            CloseCurrentFile();

            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".stf";
            dlg.Filter = _filter;

            var result = dlg.ShowDialog();

            if (result == true)
            {
                if (dlg.FilterIndex == 1)
                    SetFileManager(true);
                else
                    SetFileManager(false);

                _fileMgr.FilePath = dlg.FileName;

                try
                {
                    NoteBody.Text = _fileMgr.OpenFile();
                }
                catch
                {
                    
                    MessageBox.Show("Unable to decrypt this text file. Verify your key/password is correct.");
                }

                return;
            }

            _fileMgr = null;
        }

        private void OpenFile(string path)
        {
            if (path.EndsWith(".stf"))
                SetFileManager(true);
            else
                SetFileManager(false);

            _fileMgr.FilePath = path;

            try
            {
                NoteBody.Text = _fileMgr.OpenFile();
            }
            catch
            {
                MessageBox.Show("Unable to decrypt this text file. Verify your key/password is correct.");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFile();
        }

        private void SetFileManager(bool isSecure)
        {
            if (_fileMgr == null)
            {
                if (isSecure)
                {
                    string password = null;
                    var useContainer = false;
                    var rsaPath = "";
                    if (User.Default.RSAStore == "Container")
                    {
                        useContainer = true;
                        rsaPath = User.Default.RSAKeyContainer;
                    }
                    else
                        rsaPath = User.Default.RSAKeyPath;

                    var kt = (KeyType)User.Default.KeyType;
                    if(kt == KeyType.Password)
                        password = GetPassword("Enter password used to encrypt the file");
                    else if(kt == KeyType.KeyFile)
                        password = GetPassword("Enter password used to protect key file (leave blank for unencrypted key)");

                    _fileMgr = new SecureTextFileManager(kt, User.Default.AESKeyPath, useContainer, rsaPath, password, User.Default.PasswordSalt);
                }
                else
                    _fileMgr = new PlainTextFileManager();
            }

        }

        private string GetPassword(string messageText)
        {
            var dlg = new PasswordPrompt(messageText);
            dlg.ShowDialog();
            return dlg.Password;
        }

        private void SaveFile()
        {
            if (_fileMgr != null && _fileMgr.FilePath != null)
                _fileMgr.SaveFile(NoteBody.Text);
            else
            {
                var dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.DefaultExt = ".stf";
                dlg.Filter = _filter;

                var result = dlg.ShowDialog();

                if (result == true)
                {
                    if (dlg.FilterIndex == 1)
                        SetFileManager(true);
                    else
                        SetFileManager(false);

                    _fileMgr.FilePath = dlg.FileName;

                    _fileMgr.SaveFile(NoteBody.Text);
                    return;
                }

                _fileMgr = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseCurrentFile();
        }

        private bool CloseCurrentFile()
        {
            if (!String.IsNullOrEmpty(NoteBody.Text))
            {
                var mbr = MessageBox.Show("Save opened file before closing?", "Confirm Save", MessageBoxButton.YesNoCancel);
                switch (mbr)
                {
                    case MessageBoxResult.Yes:
                        SaveFile();
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return false;
                }
            }

            _fileMgr = null;
            NoteBody.Text = String.Empty;
            return true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !CloseCurrentFile();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsPage();
            settings.ShowDialog();

        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFile();
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFile();
        }

        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CloseCurrentFile();
        }
    }
}
