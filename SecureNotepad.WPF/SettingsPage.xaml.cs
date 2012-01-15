using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SecureNotepad.Core.CryptoExtensions;
using SecureNotepad.Core.FileManagers;

namespace SecureNotepad.WPF
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsPage : Window
    {
        private KeyType _keyType;
        public SettingsPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(SettingsPage_Loaded);
        }

        void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            _keyType = (KeyType)User.Default.KeyType;
            switch (_keyType)
            {
                case KeyType.RsaEncryptedKeyFile:
                    UseRSAKey.IsChecked = true;
                    break;
                case KeyType.KeyFile:
                    UseFileKey.IsChecked = true;
                    break;
                default:
                    UsePassKey.IsChecked = true;
                    break;
            }

            AESKeyPath.Text = User.Default.AESKeyPath;
            if (User.Default.RSAStore == "Container")
                RSAKeyContainerRadio.IsChecked = true;
            else
                RSAKeyPathRadio.IsChecked = true;

            RSAKeyContainer.Text = User.Default.RSAKeyContainer;
            RSAKeyPath.Text = User.Default.RSAKeyPath;
        }

        private void EncryptionType_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
                return;

            var rb = e.Source as RadioButton;
            switch (rb.Name)
            {
                case "UseRSAKey":
                    RSAKeyBox.IsEnabled = true;
                    AESKeyBox.IsEnabled = true;
                    User.Default.KeyType = Convert.ToByte(KeyType.RsaEncryptedKeyFile);
                    break;
                case "UseFileKey":
                    RSAKeyBox.IsEnabled = false;
                    AESKeyBox.IsEnabled = true;
                    User.Default.KeyType = Convert.ToByte(KeyType.KeyFile);
                    break;
                default:
                    RSAKeyBox.IsEnabled = false;
                    AESKeyBox.IsEnabled = false;
                    User.Default.KeyType = Convert.ToByte(KeyType.Password);
                    break;
            }

            _keyType = (KeyType)User.Default.KeyType;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var saltBytes = new byte[16];

            if (String.IsNullOrEmpty(User.Default.PasswordSalt))
                saltBytes = RNGExtensions.GetRandomBytes(16);
            else
                saltBytes = Convert.FromBase64String(User.Default.PasswordSalt);

            if (!File.Exists(AESKeyPath.Text))
            {
                var k = new byte[32];

                switch (_keyType)
                {
                    case KeyType.KeyFile:
                        k = RNGExtensions.GetRandomBytes(32);
                        var pwd = GetPassword("Enter password to protect key file (leave blank for unencrypted key)");
                        if (!String.IsNullOrEmpty(pwd))
                            k = k.Encrypt(pwd.GetKeyFromPassphrase(32, saltBytes));

                        File.WriteAllBytes(AESKeyPath.Text, k);
                        break;
                    case KeyType.RsaEncryptedKeyFile:
                        if (User.Default.RSAStore == "Container")
                            k = k.Encrypt(RSAKeyContainer.Text.ExportRSAKey());
                        else
                        {
                            File.WriteAllText(RSAKeyPath.Text, RSAExtensions.ExportRSAKeyAsXml());
                            k = k.Encrypt(RSAKeyPath.Text.ExportRSAKeyFromXml());
                        }

                        File.WriteAllBytes(AESKeyPath.Text, k);
                        break;
                    
                }

                
            }
            User.Default.AESKeyPath = AESKeyPath.Text;
            User.Default.RSAKeyContainer = RSAKeyContainer.Text;
            User.Default.RSAKeyPath = RSAKeyPath.Text;
            User.Default.PasswordSalt = Convert.ToBase64String(saltBytes);
            User.Default.FirstLaunch = false;
            User.Default.Save();


            Close();
        }

        private string GetPassword(string messageText = null)
        {
            var dlg = new PasswordPrompt(messageText);
            dlg.ShowDialog();
            return dlg.Password;
        }

        private void RSAKeyStore_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
            {
                RSAKeyPath.IsEnabled = false;
                RSAKeyPathBrowse.IsEnabled = false;
                return;
            }

            var rb = e.Source as RadioButton;
            switch (rb.Name)
            {
                case "RSAKeyContainerRadio":
                    RSAKeyContainer.IsEnabled = true;
                    RSAKeyPath.IsEnabled = false;
                    RSAKeyPathBrowse.IsEnabled = false;
                    User.Default.RSAStore = "Container";
                    break;
                case "RSAKeyPathRadio":
                    RSAKeyContainer.IsEnabled = false;
                    RSAKeyPath.IsEnabled = true;
                    RSAKeyPathBrowse.IsEnabled = true;
                    User.Default.RSAStore = "File";
                    break;
            }
        }

        private void AESKeyPathBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.CheckFileExists = false;
            dlg.OverwritePrompt = false;

            var result = dlg.ShowDialog();
            if (!result.HasValue || result.Value == false)
                return;

            AESKeyPath.Text = dlg.FileName;
        }

        private void RSAKeyPathBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.CheckFileExists = false;
            dlg.OverwritePrompt = false;

            var result = dlg.ShowDialog();
            if (!result.HasValue || result.Value == false)
                return;

            RSAKeyPath.Text = dlg.FileName;
        }

        private void RSAKeyExport_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.CheckFileExists = false;
            dlg.OverwritePrompt = true;

            var result = dlg.ShowDialog();
            if (!result.HasValue || result.Value == false)
                return;

            File.WriteAllText(dlg.FileName, RSAKeyContainer.Text.ExportRSAKeyAsXml());

            MessageBox.Show("RSA Public/Private key export complete!");
        }


    }
}
