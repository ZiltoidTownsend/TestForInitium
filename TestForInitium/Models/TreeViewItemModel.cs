using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media.Imaging;
using TestForInitium.ViewModels;

namespace TestForInitium.Models
{
    public class TreeViewItemModel : BaseInpc
    {
        string _name;
   
        public TreeViewItemModel()
            {
                Children = new ObservableCollection<TreeViewItemModel>();
            }

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; Set(ref _isSelected, value); }
        }


        public string Name
            {
                get { return _name; }
                set { _name = value; Set(ref _name, value); }
            }

        public ObservableCollection<TreeViewItemModel> Children { get; set; }
        public BitmapSource Source { get; set; }
        public double Size { get; set; }
        public string MimeType { get; set; }

    }
}
