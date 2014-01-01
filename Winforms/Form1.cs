using System;
using System.Windows.Forms;

namespace Winforms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonClickLabel.Text = "Button Clicked!";
        }
    }
}
