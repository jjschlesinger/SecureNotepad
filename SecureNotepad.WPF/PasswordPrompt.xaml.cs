using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace SecureNotepad.WPF
{
    /// <summary>
    /// Interaction logic for PasswordPrompt.xaml
    /// </summary>
    public partial class PasswordPrompt : Window
    {
        public string Password { get { return PasswordBox.Password; } }
        private string _messageText;
        private bool _newPassword;
        private bool _passwordMatchError;

        public PasswordPrompt()
        {
            InitializeComponent();

        }

        public PasswordPrompt(string messageText, bool newPassword) : this()
        {
            _messageText = messageText;
            _newPassword = newPassword;
        }

        private void PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.Key != Key.Enter)
                return;

            if (_passwordMatchError)
            {
                _passwordMatchError = false;
                return;
            }

            if (_newPassword && !PasswordBox.Password.Equals(PasswordConfirmBox.Password))
            {
                _passwordMatchError = true;
                //RegisterPaswordBox(true);
                MessageBox.Show("Passwords don't match!", "Invalid Password", MessageBoxButton.OK, MessageBoxImage.Error);
                //RegisterPaswordBox(false);
                return;
            }

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_newPassword)
            {
                PasswordConfirmPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            MessageText.Text = _messageText;
            if (MessageText.Text.Length > 0)
                MessageText.Visibility = System.Windows.Visibility.Visible;
            else
                MessageText.Visibility = System.Windows.Visibility.Collapsed;
            
            PasswordBox.Focus();

            RegisterPaswordBox();
        }
  
        private void RegisterPaswordBox(bool unreg = false)
        {
            if (!unreg)
            {
                PasswordBox.KeyUp += PasswordBox_KeyUp;
                PasswordConfirmBox.KeyUp += PasswordBox_KeyUp;
            }
            else
            {
                PasswordBox.KeyUp -= PasswordBox_KeyUp;
                PasswordConfirmBox.KeyUp -= PasswordBox_KeyUp;
            }
        }
    }
}
