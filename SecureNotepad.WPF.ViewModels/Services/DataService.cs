using System;
using System.Collections.Generic;
using System.Linq;
using SecureNotepad.Core.Net.SkyDrive;

namespace SecureNotepad.WPF.ViewModels.Services
{
    class DataService : IDataService
    {
        public string AccessToken { get; set; }

        public IEnumerable<BaseItem> GetFolderItems(string folderId)
        {
            throw new NotSupportedException();
        }

        public void GetFolderItemsAsync(Action<IEnumerable<BaseItem>> callback, string folderId = null)
        {
            if(String.IsNullOrEmpty(AccessToken))
                throw new ArgumentNullException("AccessToken");

            var sdClient = new SkyDriveClient(AccessToken);
            sdClient.GetFolderItemsAsync(callback, folderId);
        }

    }
}
