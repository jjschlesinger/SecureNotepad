using System;
using System.Linq;

namespace SecureNotepad.Core.FileManagers
{
    public interface IFileManager
    {
        string FilePath { get; set; }
        string FileContents { get; set; }
        string OpenFile();
        void LoadFile();
        void SaveFile(string contents);
        void SaveFile();
    }
}
