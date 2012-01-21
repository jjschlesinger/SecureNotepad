using System;
using System.Linq;

namespace SecureNotepad.Core.Net.SkyDrive
{
    public class FileItem : BaseItem
    {
        public int Size { get; set; }
        public int CommentsCount { get; set; }
        public bool CommentsEnabled { get; set; }

        public FileItem()
        {
            Type = "file";
        }
    }
}
