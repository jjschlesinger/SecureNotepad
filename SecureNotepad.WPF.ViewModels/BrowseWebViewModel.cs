using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using SecureNotepad.Core.Net.SkyDrive;
using SecureNotepad.Core.Settings;
using SecureNotepad.WPF.ViewModels.Services;

namespace SecureNotepad.WPF.ViewModels
{
    public class BrowseWebViewModel : ViewModelBase
    {
        private IUserSettings _userSettings;
        public IUserSettings UserSettings
        {
            get
            {
                return _userSettings;
            }

            set
            {
                Set(() => UserSettings, ref _userSettings, value);
            }
        }

        private FolderItem _currentFolder;
        public FolderItem CurrentFolder
        {
            get
            {
                return _currentFolder;
            }

            set
            {
                Set(() => CurrentFolder, ref _currentFolder, value);
            }
        }

        /// <summary>
        private List<BaseItemViewModel> _folderItems = null;
        public List<BaseItemViewModel> FolderItems
        {
            get
            {
                return _folderItems;
            }
            set
            {
                Set(() => FolderItems, ref _folderItems, value);
            }
        }

        public BrowseWebViewModel()
        {
            if (IsInDesignMode)
                FolderItems = ServiceLocator.Current.GetInstance<IDataService>().GetFolderItems().Select(i => new BaseItemViewModel(i)).ToList();
        }

        public void LoadFolder(string folderId = null)
        {
            var svc = ServiceLocator.Current.GetInstance<IDataService>();
            svc.AccessToken = _userSettings.Token.AccessToken;

            svc.GetFolderItemsAsync(GetFolderItemsAsyncComplete, folderId);
        }

        private void GetFolderItemsAsyncComplete(IEnumerable<BaseItem> folderItems)
        {
            FolderItems = folderItems.OrderBy(fi => fi.Type).Select(i => new BaseItemViewModel(i)).ToList();
        }
    }
}