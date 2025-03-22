using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai_3
{
    public partial class BAI3 : Form
    {
        public BAI3()
        {
            InitializeComponent();
        }

        private void ChooseFileBtn_Click(object sender, EventArgs e)
        {
            string result = "";
            ofd.Title = "Chọn file cần tính toán";
            ofd.ShowDialog();
            string path = ofd.FileName;
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            if (sr.EndOfStream)
            {
                MessageBox.Show("File rỗng!");
                return;
            }            
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                try
                {
                    result += line + " = " + EvaluateExpression(line) + "\n";
                }
                catch (Exception exc)
                {
                    if (exc is ArgumentException)
                    {
                        MessageBox.Show("Cú pháp không hợp lệ!");
                    }
                    else if (exc is DivideByZeroException)
                    {
                        MessageBox.Show("Không thể chia cho 0!");
                    }
                    else
                    {
                        MessageBox.Show("Lỗi không xác định!");
                    }
                    return;
                }
            }
            sr.Close();
            fs.Close();
            ofd.Title = "Chọn nơi lưu file kết quả";
            ofd.ShowDialog();
            path = ofd.FileName;
            fs = new FileStream(path, FileMode.Truncate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(result);
            sw.Close();
            fs.Close();
            MessageBox.Show("Tính toán thành công!\n" +
                "Kết quả được lưu vào " + Path.GetFullPath(path).ToString());
        }


        static string EvaluateExpression(string expression)
        {
            // Remove any whitespace from the expression
            expression = expression.Replace(" ", "");
            // Replace commas with points
            expression = expression.Replace(",", ".");

            // Regular expression to match a simple mathematical expression
            var regex = new Regex(@"^(-?\d+(\.\d+)?)([+\-*/])(-?\d+(\.\d+)?)$");
            var match = regex.Match(expression);

            if (!match.Success)
            {
                throw new ArgumentException("Invalid expression format");
            }

            // Extract the operands and operator
            double operand1 = double.Parse(match.Groups[1].Value);
            string operatorSymbol = match.Groups[3].Value;
            double operand2 = double.Parse(match.Groups[4].Value);

            // Perform the calculation based on the operator
            double result;
            switch (operatorSymbol)
            {
                case "+":
                    result = operand1 + operand2;
                    break;
                case "-":
                    result = operand1 - operand2;
                    break;
                case "*":
                    result = operand1 * operand2;
                    break;
                case "/":
                    if (operand2 == 0)
                    {
                        throw new DivideByZeroException();
                    }
                    result = operand1 / operand2;
                    break;
                default:
                    throw new ArgumentException();
            }

            return result.ToString();
        }
    }
}
