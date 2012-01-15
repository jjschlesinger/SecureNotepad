using System;
using System.Linq;
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

        public PasswordPrompt()
        {
            InitializeComponent();
        }

        public PasswordPrompt(string messageText) : this()
        {
            _messageText = messageText;
        }

        private void PasswordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.Enter)
                return;

            if (!PasswordBox.Password.Equals(PasswordConfirmBox.Password))
            {
                MessageBox.Show("Passwords don't match!", "Invalid Password", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MessageText.Text = _messageText;
            if (MessageText.Text.Length > 0)
                MessageText.Visibility = System.Windows.Visibility.Visible;
            else
                MessageText.Visibility = System.Windows.Visibility.Collapsed;
            
            PasswordBox.Focus();
        }
    }
}
