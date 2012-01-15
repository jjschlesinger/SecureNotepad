using System;
using System.Linq;

namespace SecureNotepad.Core.FileManagers
{
    public interface IFileManager
    {
        string FilePath { get; set; }
        string OpenFile();
        void SaveFile(string contents);
    }
}
