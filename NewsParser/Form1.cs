using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace NewsParser
{
    public partial class Form1 : Form
    {   
        enum Actions {AddSymbol, RemoveSymbol, AddNews, RemoveNews, UpdateNewsVolatile, UpdateNewsReverse};
        private string actualNewsFilename = "actualNews.txt";
        private string actualSymbolsFilename = "actualSymbols.txt";
        string directory;
        private Filter filter;
        private Parser parser;
        private Decryptor decryptor;
        private Writer writer;
        private string path = "E:\test.txt";  
        private List<string> poolSymbolsList = new List<string>();
        //private List<string> actualSymbolsList = new List<string>();
        public List<string> poolNewsList = new List<string>();
        //public List<string> actualNewsList = new List<string>();

        public List<actualNewsItem> actualNewsItemList = new List<actualNewsItem>();
        private void onApplicationExit(object sender, EventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter(directory + actualNewsFilename, false);
                foreach (string item in actualNewsListBox.Items)
                {
                    sw.WriteLine(item);
                }
                sw.Flush();
                sw.Close();
                sw = new StreamWriter(directory + actualSymbolsFilename, false);
                foreach (string item in actualSymbolListBox.Items)
                {
                    sw.WriteLine(item);
                }
                sw.Flush();
                sw.Close();
            }
            catch (Exception exc)
            {   
                actualNewsFilename = actualNewsFilename + DateTime.Now.Minute;
                actualSymbolsFilename = actualSymbolsFilename + DateTime.Now.Minute;
                onApplicationExit(null, null);
            }
        }
        public Form1()
        {
            directory = AppDomain.CurrentDomain.BaseDirectory;
            InitializeComponent();
            Application.ApplicationExit += onApplicationExit;
            filter = new Filter();
            parser = new Parser();
            decryptor = new Decryptor(parser,filter);
            writer = new Writer(path);
            checkUpdates(null, null);
            System.Timers.Timer updateTimer = new System.Timers.Timer(15 * 60 * 60 * 1000);
            updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(checkUpdates);
            poolNewsListBox.DoubleClick += addActualNews;
            actualNewsListBox.DoubleClick += removeActualNews;
            actualNewsListBox.SelectedIndexChanged += newsListBox_SelectedIndexChanged;
            poolSymbolListBox.DoubleClick += addActualSymbol;
            actualSymbolListBox.DoubleClick += removeActualSymbol;
            highRadioButton.CheckedChanged += radioButton_CheckedChanged;
            midRadioButton.CheckedChanged += radioButton_CheckedChanged;
            lowRadioButton.CheckedChanged += radioButton_CheckedChanged;
            reverse_checkBox.CheckedChanged += reverse_checkBox_CheckedChanged;
            ReadData();
        }

        private void ReadData()
        {
                StreamReader sr = null;
            if (System.IO.File.Exists(directory + actualNewsFilename))
                try
            {
                sr = new StreamReader(directory + actualNewsFilename);
                string line = sr.ReadLine();
                while (line != null)
                {
                    actualNewsListBox.Items.Add(line);
                    actualNewsItem item = new actualNewsItem(getNews(line), reverseGetVolatile(line), getReverse(line), getSymbol(line));
                    actualNewsItemList.Add(item);
                    updateFilter(Actions.AddNews, item);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception exc)
            {
                actualNewsListBox.Items.Add("Не удалось считать актуальные новости!");
                sr.Close();
            }
            if (System.IO.File.Exists(directory + actualSymbolsFilename))
                try 
            {
                sr = new StreamReader(directory + actualSymbolsFilename);
                string line = sr.ReadLine();
                while (line != null)
                {
                    actualSymbolListBox.Items.Add(line);
                    updateFilter(Actions.AddSymbol, line);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception exc)
            {
                actualSymbolListBox.Items.Add("Не удалось считать актуальные символы!");
                sr.Close();
            }
        }

        void reverse_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (actualNewsListBox.SelectedIndex >= 0)
            {
                switch (getReverse(actualNewsListBox.SelectedItem.ToString()))
                {
                    case true:
                        if (!reverse_checkBox.Checked)
                            actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("True", "False");
                        break;
                    case false:
                        if (reverse_checkBox.Checked)
                            actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("False", "True");
                        break;
                    default:
                        break;
                }

            }
        }

        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (actualNewsListBox.SelectedIndex >= 0)
            {
                switch (reverseGetVolatile(actualNewsListBox.SelectedItem.ToString()))
                {
                    case "High":
                        
                        if (!highRadioButton.Checked)
                        {
                            if (midRadioButton.Checked)
                            {
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("High", "Mid");
                                findActualNewsItem(actualNewsListBox.SelectedItem.ToString()).volatiled = "Mid";
                            }
                            if (lowRadioButton.Checked)
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("High", "Low");
                                findActualNewsItem(actualNewsListBox.SelectedItem.ToString()).volatiled = "Low";
                        }
                        break;
                    case "Mid":
                        if (!midRadioButton.Checked)
                        {
                            if (highRadioButton.Checked)
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("Mid", "High");
                            if (lowRadioButton.Checked)
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("Mid", "Low");
                        }
                        break;
                    case "Low":
                        if (!lowRadioButton.Checked)
                        {
                            if (midRadioButton.Checked)
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("Low", "Mid");
                            if (highRadioButton.Checked)
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("Low", "High");
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private actualNewsItem findActualNewsItem(string p)
        {
            throw new NotImplementedException();
        }

        void newsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (actualNewsListBox.SelectedIndex >= 0)
            {
               switch (reverseGetVolatile(actualNewsListBox.SelectedItem.ToString()))
               {
                   case "High":
                       highRadioButton.Checked = true;
                       break;
                   case "Mid":
                       midRadioButton.Checked = true;
                       break;
                   case "Low":
                       lowRadioButton.Checked = true;
                       break;
                   default:
                       break;
               }
                if (getReverse(actualNewsListBox.SelectedItem.ToString()))
                {
                    reverse_checkBox.Checked = true;
                }
                else
                {
                    reverse_checkBox.Checked = false;
                }
            }
        }

        private string reverseGetVolatile(string p)
        {
            if (p.Contains("| High"))
                return "High";
            if (p.Contains("| Mid"))
                return "Mid";
            if (p.Contains("| Low"))
                return "Low";
            return "High";
        }

        private bool getReverse(string p)
        {
            if (p.Contains("| False"))
                return false;
            if (p.Contains("| True"))
                return true;
            return false;
        }

        private void removeActualSymbol(object sender, EventArgs e)
        {
            updateFilter(Actions.RemoveSymbol, actualSymbolListBox.SelectedItem.ToString());
            actualSymbolListBox.Items.Remove(actualSymbolListBox.SelectedItem.ToString());    
        }

        private void updateFilter(Actions action, object element)
        {
            switch (action)
            {
                case Actions.AddSymbol:
                    filter.addSymbol(element.ToString());
                    break;
                case Actions.RemoveSymbol:
                    filter.removeSymbol(element.ToString());
                    break;
                case Actions.AddNews:             
                    filter.addNews((actualNewsItem) element);
                    break;
                case Actions.RemoveNews:
                    filter.removeNews((actualNewsItem) element);
                    break;
                default:
                    break;
            }
        }

        private void addActualSymbol(object sender, EventArgs e)
        {
            if (!actualSymbolListBox.Items.Contains(poolSymbolListBox.SelectedItem.ToString()))
            {
                actualSymbolListBox.Items.Add(poolSymbolListBox.SelectedItem.ToString());
                updateFilter(Actions.AddSymbol, poolSymbolListBox.SelectedItem.ToString());
            }
            
        }

        private void removeActualNews(object sender, EventArgs e)
        {
            foreach (actualNewsItem item in actualNewsItemList)
            {
                if (getNews(actualNewsListBox.SelectedItem.ToString()).Contains(item.news))
                {   
                    actualNewsItemList.Remove(item);
                    updateFilter(Actions.RemoveNews, item);
                    actualNewsListBox.Items.Remove(actualNewsListBox.SelectedItem.ToString());
                    break;
                }
            }
            
        }

        private void addActualNews(object sender, EventArgs e)
        {
            actualNewsItem item = new actualNewsItem();
            string tempString = poolNewsListBox.SelectedItem.ToString();
            item.symbol = getSymbol(tempString);
            item.news = getNews(tempString);
            item.volatiled = getVolatile();
            item.reverse = getReverse();
            bool find = false;
            foreach (actualNewsItem fItem in actualNewsItemList)
            {
                if (fItem.news.Equals(item.news) && fItem.symbol.Equals(item.symbol))
                {
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                updateFilter(Actions.AddNews, item);
                item.idx = actualNewsItemList.Count;     
                actualNewsListBox.Items.Insert(actualNewsItemList.Count, item.symbol + " | " + item.news + " | " + item.volatiled + " | " + item.reverse);
                actualNewsItemList.Add(item);
            }
        }

        private string getNews(string tempString)
        {
            if(tempString.IndexOf("|",5) < 0)
                return tempString.Substring(6);
            return tempString.Substring(6, tempString.IndexOf("|", 6) - 1 );
        }

        private string getSymbol(string tempString)
        {
            return tempString.Substring(0, 3);
        }

        private bool getReverse()
        {
            if (reverse_checkBox.Checked)
            {
                return true;
            }
            return false;
        }

        private string getVolatile()
        {
            if (highRadioButton.Checked)
            {
                return "High";
            }
            else if (midRadioButton.Checked)
            {
                return "Mid";
            }
            else if (lowRadioButton.Checked)
            {
                return "Low";
            }
            else return "High";
        }

        private void checkUpdates(Object source, ElapsedEventArgs e)
        {
            decryptor.parsedItems = parser.parse();
            decryptor.getAdvises();
            //Console.WriteLine(decryptor.parsedItems.Count);
            foreach (ParserItem item in decryptor.parsedItems)
            {
                if (!poolNewsListBox.Items.Contains(item.symbol + " | " + item.news))
                {
                    poolNewsListBox.Items.Add(item.symbol + " | " + item.news);
                }
                if (!poolSymbolListBox.Items.Contains(item.symbol))
                {
                    poolSymbolListBox.Items.Add(item.symbol);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            Filter filter = new Filter();
            Parser parser = new Parser();
            try
            {
                parser.parse();
            }
            catch (Exception exc) 
            {
                label1.Text = exc.Message;
                label1.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void volatileGroup_Enter(object sender, EventArgs e)
        {

        }


    }
}
