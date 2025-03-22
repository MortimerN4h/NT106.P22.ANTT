using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAI1
{
    public partial class BAI1: Form
    {
        public BAI1()
        {
            InitializeComponent();
        }

        private void ReadBtn_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
            string path = ofd.FileName;
            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            StreamReader streamReader = new StreamReader(fileStream);
            string content = streamReader.ReadToEnd();
            streamReader.Close();
            fileStream.Close();
            richTextBox1.Text = content;
        }

        private void WriteBtn_Click(object sender, EventArgs e)
        {
            ofd.ShowDialog();
            string path = ofd.FileName;
            FileStream fileStream = new FileStream(path, FileMode.Truncate);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.Write(richTextBox1.Text.ToUpper());
            streamWriter.Close();
            fileStream.Close();
            MessageBox.Show("Ghi File thành công!");
        }
    }
}
