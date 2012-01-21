using System;
using System.Collections.Generic;
using System.Linq;
using SecureNotepad.Core.Net.SkyDrive;

namespace SecureNotepad.WPF.ViewModels.Services
{
    class DesignService : IDataService
    {
        public string AccessToken { get;set; }

        public IEnumerable<BaseItem> GetFolderItems(string folderId)
        {
            yield return GetRandomItem<FolderItem>("Folder 1");
            yield return GetRandomItem<FolderItem>("Folder 2");
            yield return GetRandomItem<FolderItem>("Folder 3 with a very long name");
            yield return GetRandomItem<FolderItem>("Folder 4");
            yield return GetRandomItem<FileItem>("File 1");
            yield return GetRandomItem<FileItem>("File 2");
            yield return GetRandomItem<FileItem>("File 3");
        }

        private static BaseItem GetRandomItem<T>(string name) where T : BaseItem, new()
        {
            var r = new Random();
            
            BaseItem i = new T();
            i.Id = Guid.NewGuid().ToString();
            i.Name = name;
            i.Created = DateTime.Now.AddDays(-r.Next(500));
            i.Updated = DateTime.Now.AddDays(-r.Next(25));

            if(typeof(T) == typeof(FileItem))
            {
                var fi = i as FileItem;
                fi.Size = r.Next(500, 5000000);
            }

            return i;
        }

        public void GetFolderItemsAsync(Action<IEnumerable<BaseItem>> callback, string folderId = null)
        {
            throw new NotSupportedException();
        }

    }
}
