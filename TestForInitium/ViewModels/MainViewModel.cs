using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using TestForInitium.Models;

namespace TestForInitium.ViewModels
{
    public class MainViewModel
    {
        public string path { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        public ObservableCollection<string> MimeTypes { get; set; }
        public double SizeFiles { get; set; }
        public int CountFiles { get; set; }
        public ObservableCollection<string> Directories { get; set; } = new ObservableCollection<string>();
        public TreeViewModel Tree { get; }

        public MainViewModel()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            path = directoryInfo.Parent.FullName;
            Tree = new TreeViewModel(path);
            SizeFiles = Tree.StatisticDict.Sum(i => i.SizeAllFiles);
            CountFiles = Tree.StatisticDict.Sum(i => i.CountFiles);
        }
    }
}
