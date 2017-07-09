using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dolarbulucu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string gelen = textBox1.Text;
            string[] list = gelen.Split(' ');
            string sonucislem = "";
            double sonuc = 0;          
          
            foreach (var item in list)
            {
                if (item.Contains(birimtxt.Text))
                {
                    string para = item.Replace(birimtxt.Text, "").Replace(birimaditxt.Text,"");                    
                    sonucislem += para+"+";
                    sonuc += double.Parse(para);
                }

            }
            MessageBox.Show(sonucislem.Remove(sonucislem.Count()-1) + "=" + sonuc.ToString());

        }

       
    }
}
