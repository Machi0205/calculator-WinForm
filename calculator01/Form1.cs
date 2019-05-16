using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calculator01
{
    public partial class MachiCaculator : Form
    {
        public char[] signs = { '+', '-', '*', '/' };
        public double[] num;//有哪些數字
        public char[] sign;//有哪些運算符號
        public int[] sign_position;//運算符號在哪些位置，分割數字用
        public MachiCaculator()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            textBox1.Text += button.Text;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void Equal_Click(object sender, EventArgs e)
        {
            string formula = textBox1.Text;//輸入運算
            char[] char_formula = formula.ToCharArray();//轉為字元陣列
            sign = new char[Num_and_Sign(char_formula)];//運算符號陣列長度
            sign_position = new int[sign.Length + 2];//運算符號位置的陣列長度
            num = new double[sign.Length + 1];//數字的陣列長度
            Sign_Position(char_formula);
            textBox2.Text = formula;
            textBox1.Text = Convert.ToString(Answer());
        }
        //判斷有幾個運算符號來指定陣列長度
        public int Num_and_Sign(char[] formula)
        {
            int count = 0;
            for (int i = 0; i < formula.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (formula[i] == signs[j])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        //第幾位是運算符號，分別是什麼運算符號
        //分割數字
        public void Sign_Position(char[] formula)
        {
            string number;
            for (int i = 0, k = 0; i < formula.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //第i位是運算符號時執行
                    if (formula[i] == signs[j])
                    {
                        sign[k] = signs[j];//將運算符號存在陣列sign
                        sign_position[k + 1] = i;//判斷第幾位是運算符號，以便分割數字
                        k++;
                    }
                }
            }

            //設定sign_position頭尾的數字
            sign_position[0] = -1;
            sign_position[sign_position.Length - 1] = formula.Length;

            //分割數字後存進num陣列
            for (int i = 0; i < sign_position.Length - 1; i++)
            {
                number = new string(formula, sign_position[i] + 1, sign_position[i + 1] - sign_position[i] - 1);//分割數字
                num[i] = Convert.ToDouble(number);//存進陣列
            }
        }

        //先乘除後加減，回傳答案
        public double Answer()
        {
            int count = 0;//已經連續做了幾個乘除運算
            //先乘除
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == '*')
                {
                    num[i - count] = num[i - count] * num[i + 1];
                    count++;
                }
                else if (sign[i] == '/')
                {
                    num[i - count] = num[i - count] / num[i + 1];
                    count++;
                }
                else//若沒有連續乘除，count歸零
                {
                    count = 0;
                }
            }
            //後加減
            double ans = num[0];
            for (int i = 0; i < sign.Length; i++)
            {
                if (sign[i] == '+')
                {
                    ans += num[i + 1];
                }
                else if (sign[i] == '-')
                {
                    ans -= num[i + 1];
                }
            }
            return ans;
        }
    }
}
