using System;
using System.Linq;

namespace SecureNotepad.Core.UI
{
    public class DialogResult
    {
        public SecureFileType SelectedFileType { get; set; }
        public string FilePath { get; set; }
        public string Password { get; set; }
        public FileDialogType DialogType { get; set; }
    }
}
