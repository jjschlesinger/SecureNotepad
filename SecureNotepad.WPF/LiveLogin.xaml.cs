using System;
using System.Linq;
using System.Windows;
using SecureNotepad.Core.Extensions;
using SecureNotepad.Core.Net.OAuth;
using SecureNotepad.WPF.ViewModels;

namespace SecureNotepad.WPF
{
    /// <summary>
    /// Interaction logic for LiveLogin.xaml
    /// </summary>
    public partial class LiveLogin : Window
    {
        public OAuthToken Token { get; private set; }

        public LiveLogin(string navigateUrl)
        {
            InitializeComponent();
            Closing += new System.ComponentModel.CancelEventHandler(LiveLogin_Closing);
            LiveLoginWeb.Navigate(navigateUrl);
        }

        void LiveLogin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LiveLoginWeb.Source != null)
            {
                var nvp = LiveLoginWeb.Source.Fragment.Remove(0, 1).FromQueryStringToCollection();
                Token = new OAuthToken
                {
                    AccessToken = nvp["access_token"],
                    TokenType = nvp["token_type"],
                    ExpiresIn = Convert.ToInt32(nvp["expires_in"]),
                    AuthToken = nvp["authentication_token"]
                };

            }
        }

    }
}
