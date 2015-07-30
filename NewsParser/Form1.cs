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
        private string advisePath;
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
            advisePath = directory + "advise.txt";
            InitializeComponent();
            Application.ApplicationExit += onApplicationExit;
            filter = new Filter(this);
            parser = new Parser();
            decryptor = new Decryptor(parser,filter);
            writer = new Writer(advisePath);
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
            writer.write(decryptor.getAdvises());
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
                    actualNewsItem item = new actualNewsItem(getNews(line), getVolatile(line), getReverse(line), getSymbol(line));
                    updateFilter(Actions.AddNews, line);
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
                        {
                            actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("True", "False");
                            updateFilter(Actions.UpdateNewsReverse, actualNewsListBox.SelectedItem.ToString());
                        }
                        break;
                    case false:
                        if (reverse_checkBox.Checked)
                        {
                            actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("False", "True");
                            updateFilter(Actions.UpdateNewsReverse, actualNewsListBox.SelectedItem.ToString());
                        }
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
                switch (getVolatile(actualNewsListBox.SelectedItem.ToString()))
                {
                    case "High":
                        
                        if (!highRadioButton.Checked)
                        {
                            if (midRadioButton.Checked)             
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("High", "Mid");          
                            if (lowRadioButton.Checked)
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("High", "Low");
                            updateFilter(Actions.UpdateNewsVolatile, actualNewsListBox.SelectedItem.ToString());
                        }
                        break;
                    case "Mid":
                        if (!midRadioButton.Checked)
                        {
                            if (highRadioButton.Checked)
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("Mid", "High");
                            if (lowRadioButton.Checked)
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("Mid", "Low");
                            updateFilter(Actions.UpdateNewsVolatile, actualNewsListBox.SelectedItem.ToString());
                        }
                        break;
                    case "Low":
                        if (!lowRadioButton.Checked)
                        {
                            if (midRadioButton.Checked)
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("Low", "Mid");
                            if (highRadioButton.Checked)
                                actualNewsListBox.Items[actualNewsListBox.SelectedIndex] = actualNewsListBox.SelectedItem.ToString().Replace("Low", "High");
                            updateFilter(Actions.UpdateNewsVolatile, actualNewsListBox.SelectedItem.ToString());
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        void newsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (actualNewsListBox.SelectedIndex >= 0)
            {
               switch (getVolatile(actualNewsListBox.SelectedItem.ToString()))
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

        public string getVolatile(string p)
        {
            if (p.Contains("| High"))
                return "High";
            if (p.Contains("| Mid"))
                return "Mid";
            if (p.Contains("| Low"))
                return "Low";
            return "High";
        }

        public bool getReverse(string p)
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

        private void updateFilter(Actions action, string element)
        {
            switch (action)
            {
                case Actions.AddSymbol:
                    filter.addSymbol(element);
                    break;
                case Actions.RemoveSymbol:
                    filter.removeSymbol(element);
                    break;
                case Actions.AddNews:
                    filter.addNews(element);
                    break;
                case Actions.RemoveNews:
                    filter.removeNews(element);
                    break;
                case Actions.UpdateNewsReverse:
                    filter.updateReverse(element);
                    break;
                case Actions.UpdateNewsVolatile:
                    filter.updateVolatile(element);
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
            if (actualNewsListBox.SelectedIndex >= 0)
            {   
                updateFilter(Actions.RemoveNews, actualNewsListBox.SelectedItem.ToString());
                actualNewsListBox.Items.Remove(actualNewsListBox.SelectedItem.ToString());
            }         
        }

        private void addActualNews(object sender, EventArgs e)
        {
            bool find = false;
            foreach (string item in actualNewsListBox.Items)
            {
                Console.WriteLine(item);
                Console.WriteLine(poolNewsListBox.SelectedItem.ToString());
                if (item.Contains(poolNewsListBox.SelectedItem.ToString()))
                {
                    find = true;
                    break;
                }
            }
            if (!find)
            {
                string news = poolNewsListBox.SelectedItem.ToString();
                news = getSymbol(news) + " | " + getNews(news) + " | " + getVolatile() + " | " + getReverse();
                actualNewsListBox.Items.Add(news);
                updateFilter(Actions.AddNews, news);
            }
        }

        public string getNews(string tempString)
        {
            if(tempString.IndexOf("|",5) < 0)
                return tempString.Substring(6);
            return tempString.Substring(6, tempString.IndexOf("|", 6) - 7);
        }

        public string getSymbol(string tempString)
        {
            return tempString.Substring(0, 3);
        }

        public bool getReverse()
        {
            if (reverse_checkBox.Checked)
            {
                return true;
            }
            return false;
        }

        public string getVolatile()
        {
            if (highRadioButton.Checked)
                return "High";
            if (midRadioButton.Checked)
                return "Mid";
            if (lowRadioButton.Checked)
                return "Low";
            return "High";
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void volatileGroup_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            checkUpdates(null, null);
        }


    }
}
