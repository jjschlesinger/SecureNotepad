using System;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using SecureNotepad.WPF.ViewModels;

namespace SecureNotepad.WPF
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsPage : Window
    {
        private SettingsViewModel _vm;
        public SettingsPage()
        {


            InitializeComponent();
            Closing += new System.ComponentModel.CancelEventHandler(SettingsPage_Closing);
            _vm = DataContext as SettingsViewModel;
            Messenger.Default.Register<Boolean>(this, "GetPassword", b => GetPassword(b));
            Messenger.Default.Register<Boolean>(this, "SaveComplete", b => Close());
            
        }

        void SettingsPage_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _vm.Cleanup();
        }

        private void GetPassword(bool show)
        {
            if (!show)
                return;

            var dlg = new PasswordPrompt("Enter password to protect key file (leave blank for unencrypted key)", true);
            dlg.ShowDialog();
            //return dlg.Password;
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

        private void RegenSalt_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(User.Default.PasswordSalt);
        }


    }
}
