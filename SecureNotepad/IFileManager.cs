using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecureNotepad
{
    interface IFileManager
    {
        string FilePath { get; set; }
        string OpenFile();
        void SaveFile(string contents);
    }
}
