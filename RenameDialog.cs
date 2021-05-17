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

namespace STLViewer
{
    public partial class RenameDialog : Form
    {
        public string outputString = "";
        private string basepath = "";
        public RenameDialog()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(string inputString, string basePath)
        {
            textBox1.Text = inputString;
            textBox2.Text = inputString;
            textBox2.Select();
            basepath = basePath;

            var res = base.ShowDialog();
            outputString = textBox2.Text;
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine($"Path: {basepath}");
            Console.WriteLine($"Renaming {textBox1.Text} to {textBox2.Text}...");

            try
            {
                File.Move(basepath + textBox1.Text, basepath + textBox2.Text);
                Console.WriteLine("OK.");
                DialogResult = DialogResult.OK;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Text = ex.Message;
                textBox2.BackColor = Color.Salmon;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.BackColor = textBox1.BackColor;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) button1_Click(null, null);
            if (e.KeyCode == Keys.Escape) DialogResult = DialogResult.Cancel;
        }
    }
}
