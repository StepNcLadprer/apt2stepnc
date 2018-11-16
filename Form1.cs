using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace apt2stepnc
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        APT aptIns = new APT();

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            lblBrowser.Text = "Choose a file!";
            lblReady.Text = "";
            textBox1.Text = "";
            btnMakeStepnc.Enabled = false;
            
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            bool ok = aptIns.ReadMastercamCLFile(openFileDialog1.FileName);

            if (!ok)
                return;

            btnMakeStepnc.Enabled = true;
            lblBrowser.Text = Path.GetFileName(openFileDialog1.FileName);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblReady.Text = "";
            if (textBox1.Text == "")
            {
                MessageBox.Show("give a name to the output file!");
                return;
            }
            
            aptIns.WriteSTEPNC(aptIns.MastercamData, textBox1.Text);
            lblReady.Text = "Ready!";
        }

    }
}
