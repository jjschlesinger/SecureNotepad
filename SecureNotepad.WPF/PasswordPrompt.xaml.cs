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
        public string Password { get; private set; }
        private string _messageText;
        private bool _isNewPassword;
        private bool _passwordMatchError;

        public PasswordPrompt()
        {
            InitializeComponent();

        }

        public PasswordPrompt(string messageText, bool isNewPassword)
            : this()
        {
            _messageText = messageText;
            _isNewPassword = isNewPassword;
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

            if (_isNewPassword && !PasswordBox.Password.Equals(PasswordConfirmBox.Password))
            {
                _passwordMatchError = true;
                //RegisterPaswordBox(true);
                MessageBox.Show("Passwords don't match!", "Invalid Password", MessageBoxButton.OK, MessageBoxImage.Error);
                //RegisterPaswordBox(false);
                return;
            }

            Password = PasswordBox.Password;

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isNewPassword)
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

        private void RegisterPaswordBox()
        {
            PasswordBox.KeyDown += PasswordBox_KeyUp;
            PasswordConfirmBox.KeyDown += PasswordBox_KeyUp;

            
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            
        }
    }
}
