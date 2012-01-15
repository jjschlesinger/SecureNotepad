using System;
using System.IO;
using System.Linq;

namespace SecureNotepad.Core.FileManagers
{
    public class PlainTextFileManager : IFileManager
    {
        public PlainTextFileManager()
        {
        }

        public PlainTextFileManager(string filePath)
        {
            FilePath = filePath;
        }

        public string OpenFile(string path)
        {
            return File.ReadAllText(path);
        }

        public void SaveFile(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public string FilePath
        {
            get;
            set;
        }

        public string OpenFile()
        {
            return File.ReadAllText(FilePath);
        }

        public void SaveFile(string contents)
        {
            File.WriteAllText(FilePath ,contents);
        }
    }
}
