using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using SecureNotepad.Core.Net.SkyDrive;

namespace SecureNotepad.WPF.ViewModels
{
    public class BaseItemViewModel : ViewModelBase
    {
        public BaseItem Item { get; private set; }

        /// <summary>
        /// Sets and gets the Name property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Name
        {
            get
            {
                return Item.Name;
            }

            set
            {
                if (Item.Name == value)
                {
                    return;
                }

                Item.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public string Source
        {
        	get
            {
                if (Item.GetType() == typeof(FolderItem))
                    return "/SecureNotepad;component/Images/Folder_Closed.png";
                else
                    return "/SecureNotepad;component/Images/Generic_Document.png";
            }
        }

        public BaseItemViewModel(BaseItem model)
        {
            Item = model;
        }
    }
}
