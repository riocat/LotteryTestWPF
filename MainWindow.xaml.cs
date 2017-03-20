using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public ObservableCollection<Lottery> lotteryList;

        private void Open_report(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost/task/1");
        }

        private void Open_report2(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost/case/1");
        }

        private void Open_report3(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost/action/1");
        }

        private void getNeedData(object sender, RoutedEventArgs e)
        {
            //if (checkDataStore())
            //{
            //    return;
            //}
            //if (!File.Exists(@"C:\Users\admin\Documents\Visual Studio 2012\Projects\WpfApplication1\WpfApplication1\tempData.txt"))
            //{
            //    StreamWriter sw = File.CreateText(@"C:\Users\admin\Documents\Visual Studio 2012\Projects\WpfApplication1\WpfApplication1\tempData.txt");
            //    WebRequest wr = WebRequest.Create("http://datachart.500.com/dlt/zoushi/newinc/jbzs_foreback.php?expect=all&from=07001&to=16033");
            //    WebResponse wrp = wr.GetResponse();
            //    Stream ReceiveStream = wrp.GetResponseStream();
            //    StreamReader sr = new StreamReader(ReceiveStream);
            //    string line;
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        sw.WriteLine(line);
            //    }
            //    sr.Close();
            //    sw.Close();
            //}

            string path = Environment.CurrentDirectory;
            if (checkDataStore())
            {
                return;
            }
            string start = "07001", end = "16033";

            getTempData(path, start, end);

            getUseTxt(path, start);

            createLotterys(path,true);

            File.Delete(path + @"\tempData.txt");
            File.Delete(path + @"\useData.txt");
        }

        private void getTempData(string path, string start, string end)
        {
            if (!File.Exists(path + @"\tempData.txt"))
            {
                StreamWriter sw = File.CreateText(path + @"\tempData.txt");
                WebRequest wr = WebRequest.Create("http://datachart.500.com/dlt/zoushi/newinc/jbzs_foreback.php?expect=all&from=" + start + "&to=" + end);
                WebResponse wrp = wr.GetResponse();
                Stream ReceiveStream = wrp.GetResponseStream();
                StreamReader sr = new StreamReader(ReceiveStream);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    sw.WriteLine(line);
                }
                sr.Close();
                sw.Close();
            }
        }

        private bool checkDataStore()
        {
            LotteryDataModle ldm = LotteryDataModle.getInstance();
            int flag = ldm.getDataCount();
            if (flag != 0)
            {
                return true;
            }
            return false;
        }

        private void getUseTxt(string path, string start)
        {
            String tempDataPath = path + @"\tempData.txt";
            if (File.Exists(tempDataPath) && !File.Exists(path + @"\useData.txt"))
            {
                StreamWriter sw = File.CreateText(path + @"\useData.txt");
                StreamReader sr = new StreamReader(tempDataPath);
                string line;
                bool startMarker = false;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!startMarker && line.Contains("<td align=\"center\">" + start + " </td>"))
                        startMarker = true;

                    if (line.Contains("<!-- preview begin -->"))
                    {
                        break;
                    }
                    if (startMarker)
                    {
                        sw.WriteLine(line);
                    }
                }
                sr.Close();
                sw.Close();
            }
        }

        private void createLotterys(string path, bool firstFlag)
        {
            ObservableCollection<Lottery> lotteryList = new ObservableCollection<Lottery>();
            using (StreamReader sr = new StreamReader(path + @"\useData.txt"))
            {
                String line;
                String[] subStr = null;
                int? no = null;
                if (!firstFlag)
                {
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                }
                LotteryDataModle ldm = LotteryDataModle.getInstance();
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("align=\"center\""))
                    {
                        Match mac = Regex.Match(line, @"\d+");
                        no = int.Parse(mac.Value);
                    }
                    if (line.Contains("class=\"yl01\""))
                    {
                        subStr = line.Split(new string[] { "</td>", }, StringSplitOptions.RemoveEmptyEntries);
                        Lottery lty = new Lottery();
                        lty.No = no;
                        int index = 0;
                        foreach (string temp in subStr)
                        {
                            if (temp.Contains("chartBall01") || temp.Contains("chartBall02"))
                            {
                                string[] tempArray = temp.Split(new char[] { '>' });
                                lty[index++] = int.Parse(tempArray[1]);
                            }
                        }
                        lotteryList.Add(lty);
                    }
                }
                ldm.Insert(lotteryList);
            }

            MessageBox.Show(lotteryList.Count + "");
        }

        private void getNewData(object sender, RoutedEventArgs e)
        {
            string path = Environment.CurrentDirectory;
            StreamWriter sw = File.CreateText(path + @"\index.xml");
            WebRequest wr = WebRequest.Create("http://kaijiang.500.com/static/info/kaijiang/xml/index.xml");
            WebResponse wrp = wr.GetResponse();
            Stream ReceiveStream = wrp.GetResponseStream();
            StreamReader sr = new StreamReader(ReceiveStream);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                sw.WriteLine(line);
            }
            sr.Close();
            sw.Close();

            //XElement xe = XElement.Load(path + @"\index.xml");    
            //IEnumerable<XElement> elements = from el in xe.Elements("lottery")
            //                                 where el.
            //                                 select el;

            string latestNum = null;
            XmlDocument xd = new XmlDocument();
            XmlReader xr = XmlReader.Create(path + @"\index.xml");
            xd.Load(xr);
            XmlNodeList xlist = xd.LastChild.ChildNodes;
            foreach (XmlNode xln in xlist)
            {
                if (!xln.FirstChild.FirstChild.Value.Equals("dlt"))
                {
                    continue;
                }
                else
                {
                    foreach (XmlNode temp in xln.ChildNodes)
                    {
                        if (!temp.Name.Equals("periodicalnum"))
                        {
                            continue;
                        }
                        else
                        {
                            latestNum = temp.FirstChild.Value;
                            break;
                        }
                    }
                    break;
                }

            }
            xr.Close();

            Lottery latest = getlastRecord();


            if (latest.No != Convert.ToInt32(latestNum))
            {
                string start = latest.No + "";
                getTempData(path, start, latestNum);

                getUseTxt(path, start);

                createLotterys(path, false);

                File.Delete(path + @"\tempData.txt");
                File.Delete(path + @"\useData.txt");
            }
            File.Delete(path + @"\index.xml");
        }

        private Lottery getlastRecord()
        {
            LotteryDataModle ldm = LotteryDataModle.getInstance();
            return ldm.getlastRecord();
        }

        private void getLast100(object sender, RoutedEventArgs e)
        {
            LotteryDataModle ldm = LotteryDataModle.getInstance();
            this.resultList.ItemsSource = ldm.getLast100();
        }

    }
}
