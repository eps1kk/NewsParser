using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Form1()
        {
            InitializeComponent();
            filter = new Filter();
            parser = new Parser(filter);
            decryptor = new Decryptor(parser);
            writer = new Writer(path, decryptor);
            filter.addSymbol("EUR");
            filter.addSymbol("USD");
            foreach (string item in filter.getSymbols())
            {
                symbolList.Items.Add(item);
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


    }
}
