using HeyRed.Mime;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using TestForInitium.ViewModels;


namespace TestForInitium.Models
{
    public class TreeViewModel : BaseInpc
    {
        public List<StatiscticModel> StatisticDict { get; set; } = new List<StatiscticModel>();

        public HtmlDocument doc = new HtmlDocument();

        public TreeViewModel(string path)
        {
            TopLevelItems = new ObservableCollection<TreeViewItemModel>();

            DirectoryInfo Root = new DirectoryInfo(path);

            HtmlNode div = doc.CreateElement("div");
            div.Attributes.Add("id", "left");
            HtmlNode table = doc.CreateElement("h4");
            HtmlNode tr = doc.CreateElement("a");
            tr.InnerHtml = "Текущая директория";
            HtmlNode ul = doc.CreateElement("ul");
            table.ChildNodes.Append(tr);
            div.AppendChild(ul);

            GetAll(TopLevelItems, Root.GetDirectories(), Root.GetFiles(), ul);

            using (StreamWriter sw = new StreamWriter("table.html"))
            {
                sw.WriteLine("<link rel=\"stylesheet\" href=\"style.css\" type=\"text/css\"/>");
                div.WriteTo(sw);
                CreateTable().WriteTo(sw);
            }
        }

        public ObservableCollection<TreeViewItemModel> TopLevelItems { get; private set; }

        public void GetAll(ObservableCollection<TreeViewItemModel> DirectoriesTree, DirectoryInfo[] directories, FileInfo[] files, HtmlNode nodeRoot)
        {


            // write text, no indent :(

            if (directories != null)
            {
                foreach (var directory in directories)
                {
                    HtmlNode ul = doc.CreateElement("ul");
                    HtmlNode details = doc.CreateElement("details");
                    HtmlNode summary = doc.CreateElement("summary");
                    summary.InnerHtml = directory.Name;
                    details.AppendChild(summary);
                    details.AppendChild(ul);
                    nodeRoot.AppendChild(details);
                    DirectoriesTree.Add(new TreeViewItemModel { Children = new ObservableCollection<TreeViewItemModel>(), Name = directory.Name });
                }
                foreach (var file in files)
                {
                    HtmlNode li = doc.CreateElement("li");
                    HtmlNode a = doc.CreateElement("a");
                    a.InnerHtml = file.Name;
                    li.AppendChild(a);
                    nodeRoot.AppendChild(li);
                    //Statisctic.CountFiles++;
                    //Statisctic.SizeAllFiles += file.Length;

                    var mimeType = MimeTypesMap.GetMimeType(file.FullName);
                    if (StatisticDict.Contains(StatisticDict.Where(i => i.MimeType == mimeType).SingleOrDefault()))
                    {
                        StatisticDict.SingleOrDefault(i => i.MimeType == mimeType).CountFiles++;
                        StatisticDict.SingleOrDefault(i => i.MimeType == mimeType).SizeAllFiles += file.Length;
                    }
                    else
                        StatisticDict.Add(new StatiscticModel { CountFiles = 1, SizeAllFiles = file.Length, MimeType = mimeType });
                    var icon = Icon.ExtractAssociatedIcon(file.FullName);
                    var Source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    DirectoriesTree.Add(new TreeViewItemModel {Children = null, Name = file.Name, Source = Source, Size = file.Length, MimeType = mimeType });
                }
                for (int i = 0; i < DirectoriesTree.Count; i++)
                {
                    if (DirectoriesTree[i].Children != null)
                    {

                        GetAll(DirectoriesTree[i].Children, directories[i].GetDirectories(), directories[i].GetFiles(), nodeRoot.ChildNodes[i].ChildNodes[1]);
                        DirectoriesTree[i].Size = DirectoriesTree[i].Children.Sum(i => i.Size);
                    }
                }
            }
        }

        public HtmlNode CreateTable()
        {
            HtmlNode div = doc.CreateElement("div");
            div.Attributes.Add("id", "right");
            HtmlNode table = doc.CreateElement("table");
            HtmlNode tr = doc.CreateElement("tr");
            HtmlNode th = doc.CreateElement("th");
            th.InnerHtml = "MimeType";
            tr.AppendChild(th);
            th = doc.CreateElement("th");
            th.InnerHtml = "Количество файлов";
            tr.AppendChild(th);
            th = doc.CreateElement("th");
            th.InnerHtml = "Соотношение";
            tr.AppendChild(th);
            th = doc.CreateElement("th");
            th.InnerHtml = "Средний размер файлов";
            tr.AppendChild(th);
            table.AppendChild(tr);
            //div.AppendChild(table);
            HtmlNode td = doc.CreateElement("td");


            foreach (var stat in StatisticDict)
            {
                HtmlNode trR = doc.CreateElement("tr");
                td = doc.CreateElement("td");
                td.InnerHtml = stat.MimeType;
                trR.AppendChild(td);
                td = doc.CreateElement("td");
                td.InnerHtml = stat.CountFiles.ToString() + " / " + StatisticDict.Sum(i => i.CountFiles);
                trR.AppendChild(td);
                td = doc.CreateElement("td");
                td.InnerHtml = ((float)stat.CountFiles / StatisticDict.Sum(i => i.CountFiles) * 100).ToString() + "%";
                trR.AppendChild(td);
                td = doc.CreateElement("td");
                td.InnerHtml = ((float)stat.SizeAllFiles / stat.CountFiles).ToString() + " байт";
                trR.AppendChild(td);
                table.AppendChild(trR);
            }
            div.AppendChild(table);

            return div;
        }

    }
}
