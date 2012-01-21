using System;
using System.Linq;

namespace SecureNotepad.Core.Net.SkyDrive
{
    public class FolderItem : BaseItem
    {
        public string UploadLocation { get; set; }
        public int ChildrenCount { get; set; }

        public FolderItem()
        {
            Type = "folder";
        }
    }
}
