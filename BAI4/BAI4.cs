using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BAI4
{
    public partial class BAI4 : Form
    {
        public BAI4()
        {
            InitializeComponent();
        }

        private void chooseBtn_Click(object sender, EventArgs e)
        {
            ofd.Title = "Chọn file";
            ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;
            ofd.ShowDialog();
            string path = ofd.FileName;
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string line;
            List<string> lines = new List<string>();
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (String.IsNullOrEmpty(line))
                {
                    continue;
                }
                lines.Add(line);
            }
            sr.Close();
            fs.Close();
            List<Student> students = new List<Student>();
            while (lines.Count > 0)
            {
                Student student = new Student();
                student.MSSV = lines[0];
                student.HoTen = lines[1];
                student.DienThoai = lines[2];
                student.DiemToan = Convert.ToDouble(lines[3]);
                student.DiemVan = Convert.ToDouble(lines[4]);
                student.DTB = (student.DiemToan + student.DiemVan) / 2;
                students.Add(student);
                lines.RemoveRange(0, 5);
            }
            ofd.Title = "Lưu file";
            ofd.ShowDialog();
            path = ofd.FileName;
            fs = new FileStream(path, FileMode.Truncate);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, students);
            fs.Close();
        }

        private void readBtn_Click(object sender, EventArgs e)
        {
            ofd.Title = "Chọn file";
            ofd.ShowDialog();
            string path = ofd.FileName;
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            List<Student> students = (List<Student>)bf.Deserialize(fs);
            fs.Close();
            foreach (Student student in students)
            {
                string line = student.MSSV +
                    "\n" + student.HoTen +
                    "\n" + student.DienThoai +
                    "\n" + student.DiemToan.ToString() +
                    "\n" + student.DiemVan.ToString() +
                    "\n" + student.DTB.ToString() +
                    "\n\n";
                richTextBox1.Text += line;
            }
        }
    }
}
