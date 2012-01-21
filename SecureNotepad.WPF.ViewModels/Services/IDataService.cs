using System;
using System.Collections.Generic;
using System.Linq;
using SecureNotepad.Core.Net.SkyDrive;

namespace SecureNotepad.WPF.ViewModels.Services
{
    interface IDataService
    {
        string AccessToken { get; set; }
        IEnumerable<BaseItem> GetFolderItems(string folderId = null);
        void GetFolderItemsAsync(Action<IEnumerable<BaseItem>> callback, string folderId = null);
    }
}
