using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace SecureNotepad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] CLIArgs;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CLIArgs = e.Args;
        }
    }
}
