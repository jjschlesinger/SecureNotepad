using System;
using System.Linq;

namespace SecureNotepad.Core.Net.SkyDrive
{
    public abstract class BaseItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public FolderItem Parent { get; set; }
        public string Type { get; protected set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
