using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace NewsParser
{
    public partial class Form1 : Form
    {
        private Filter filter;
        private Parser parser;
        private Decryptor decryptor;
        private Writer writer;
        private string path = "E:\test.txt";
        private List<string> poolSymbolsList = new List<string>();
        private List<string> actualSymbolsList = new List<string>();
        private List<string> poolNewsList = new List<string>();
        private List<string> actualNewsList = new List<string>();

        public Form1()
        {
            InitializeComponent();
            filter = new Filter();
            parser = new Parser(filter);
            decryptor = new Decryptor(parser,filter);
            writer = new Writer(path, decryptor);
            checkUpd(null, null);
            System.Timers.Timer updateTimer = new System.Timers.Timer(15 * 60 * 60 * 1000);
            updateTimer.Elapsed += new System.Timers.ElapsedEventHandler(checkUpd);
            poolNewsListBox.DoubleClick += new EventHandler(addActualNews);
            newsListBox.DoubleClick += new EventHandler(removeActualNews);
            poolSymbolListBox.DoubleClick += new EventHandler(addActualSymbol);
            symbolListBox.DoubleClick += new EventHandler(removeActualSymbol);
        }

        private void removeActualSymbol(object sender, EventArgs e)
        {
            actualSymbolsList.Remove(symbolListBox.SelectedItem.ToString());
            symbolListBox.Items.Remove(symbolListBox.SelectedItem.ToString());
            updateFilter();
        }

        private void updateFilter()
        {
            filter.setSymbols(actualSymbolsList);
            filter.setNews(actualNewsList);
        }

        private void addActualSymbol(object sender, EventArgs e)
        {
            if (!actualSymbolsList.Contains(poolSymbolListBox.SelectedItem.ToString()))
            {
                actualSymbolsList.Add(poolSymbolListBox.SelectedItem.ToString());
                symbolListBox.Items.Add(poolSymbolListBox.SelectedItem.ToString());
                updateFilter();
            }
            
        }

        private void removeActualNews(object sender, EventArgs e)
        {
            actualNewsList.Remove(newsListBox.SelectedItem.ToString());
            newsListBox.Items.Remove(newsListBox.SelectedItem.ToString());
            updateFilter();
        }

        private void addActualNews(object sender, EventArgs e)
        {
            if (!actualNewsList.Contains(poolNewsListBox.SelectedItem))
            {
                actualNewsList.Add(poolNewsListBox.SelectedItem.ToString());
                newsListBox.Items.Add(poolNewsListBox.SelectedItem);
                updateFilter();
            }
            
        }
        private void checkUpd(Object source, ElapsedEventArgs e)
        {
            decryptor.parsedItems = parser.parse();
            decryptor.getAdvises();
            Console.WriteLine(decryptor.parsedItems.Count);
            foreach (ParserItem item in decryptor.parsedItems)
            {
                Console.WriteLine("tytochki!!" + item.symbol);
                if (!poolNewsList.Contains(item.news))
                {
                    poolNewsList.Add("(" + item.symbol + ")" + item.news);
                }
                if (!poolSymbolsList.Contains(item.symbol))
                {
                    poolSymbolsList.Add(item.symbol);
                }
            }
            foreach(string item in poolNewsList)
            {
                if (!poolNewsListBox.Items.Contains(item))
                {
                    poolNewsListBox.Items.Add(item);
                }
            }
            foreach(string item in poolSymbolsList)
            {
                if (!poolSymbolListBox.Items.Contains(item))
                {
                    poolSymbolListBox.Items.Add(item);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            Filter filter = new Filter();
            Parser parser = new Parser(filter);
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


    }
}
