using System;
using System.Linq;
using System.Windows;
using SecureNotepad.Core.Net.SkyDrive;
using SecureNotepad.WPF.ViewModels;

namespace SecureNotepad.WPF
{
    /// <summary>
    /// Interaction logic for BrowseSkyDrive.xaml
    /// </summary>
    public partial class BrowseSkyDrive : Window
    {
        private BrowseWebViewModel _vm;

        public BrowseSkyDrive()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = DataContext as BrowseWebViewModel;
            _vm.UserSettings = new UserSettings();
            _vm.LoadFolder();
        }

        private void FolderListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = FolderListBox.SelectedItem as BaseItemViewModel;
            var folder = item.Item as FolderItem;
            if (folder != null)
            {
                _vm.CurrentFolder = folder;
                _vm.LoadFolder(folder.Id);
                return;
            }

            //handle file selected

        }
    }
}
