using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
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

            Messenger.Default.Register<Boolean>(this, "LoadFolderComplete", b => LoadFolderComplete());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = DataContext as BrowseWebViewModel;
            _vm.UserSettings = new UserSettings();
            _vm.CurrentFolder = new FolderItem { Id = "", Name = "SkyDrive" };
            _vm.LoadFolder();
        }

        private void LoadFolderComplete()
        {
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                FolderListBox.Cursor = Cursors.Arrow;
                FolderListBox.IsEnabled = true;
            }));

        }

        private void FolderListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = FolderListBox.SelectedItem as BaseItemViewModel;
            if (item == null)
                return;

            var folder = item.Item as FolderItem;
            if (folder != null)
            {
                FolderListBox.Cursor = Cursors.Wait;
                FolderListBox.IsEnabled = false;
                folder.Parent = _vm.CurrentFolder;
                _vm.CurrentFolder = folder;
                _vm.LoadFolder(folder.Id);
                return;
            }

            //handle file selected

        }


        private void FolderListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var item = FolderListBox.SelectedItem as BaseItemViewModel;
            if (item == null)
                return;

            var file = item.Item as FileItem;

            if (file == null)
                return;

            _vm.SelectedFile = file;
        }
    }
}
