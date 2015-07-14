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
        public Form1()
        {
            InitializeComponent();
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
