using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using SecureNotepad.Core.Net.SkyDrive;
using SecureNotepad.Core.Settings;
using SecureNotepad.WPF.ViewModels.Services;

namespace SecureNotepad.WPF.ViewModels
{
    public class BrowseWebViewModel : ViewModelBase
    {
        public RelayCommand GoBackCommand { get; private set; }
        public Dictionary<FolderItem, List<BaseItemViewModel>> FolderHistory { get; set; }

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

        private FileItem _selectedFile;
        public FileItem SelectedFile
        {
            get
            {
                return _selectedFile;
            }

            set
            {
                Set(() => SelectedFile, ref _selectedFile, value);
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
            FolderHistory = new Dictionary<FolderItem, List<BaseItemViewModel>>();
            GoBackCommand = new RelayCommand(() => GoBack());
            if (IsInDesignMode)
            {
                FolderItems = ServiceLocator.Current.GetInstance<IDataService>().GetFolderItems().Select(i => new BaseItemViewModel(i)).ToList();
                CurrentFolder = new FolderItem { Name = "SkyDrive" };
                SelectedFile = new FileItem { Name = "a single file.txt" };
            }
        }

        public void GoBack()
        {
            if (_currentFolder.Parent == null)
                return;

            CurrentFolder = _currentFolder.Parent;
            FolderItems = FolderHistory[_currentFolder];
            
        }

        public void LoadFolder(string folderId = null)
        {
            var svc = ServiceLocator.Current.GetInstance<IDataService>();
            svc.AccessToken = _userSettings.Token.AccessToken;

            svc.GetFolderItemsAsync(GetFolderItemsAsyncComplete, folderId);
        }

        private void GetFolderItemsAsyncComplete(IEnumerable<BaseItem> folderItems)
        {
            FolderItems = folderItems.OrderByDescending(fi => fi.Type).Select(i => new BaseItemViewModel(i)).ToList();
            if (!FolderHistory.Any(kvp => kvp.Key.Id == _currentFolder.Id))
                FolderHistory.Add(_currentFolder, FolderItems);

            MessengerInstance.Send<Boolean>(true, "LoadFolderComplete");
        }
    }
}