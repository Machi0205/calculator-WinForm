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
        public char[] signs = { '+', '-', '*', '/', '(', ')' };
        public double[] num;//有哪些數字
        public char[] sign;//有哪些運算符號
        public int[] sign_position;//運算符號在哪些位置，分割數字用
        public int[,] sign_order;//儲存運算順序
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
            if (textBox1.Text != "")
            {
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
            }
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
            sign = new char[Num_and_Sign(char_formula)[0]];//運算符號陣列長度
            sign_order = new int[sign.Length,2];//運算順序陣列長度
            sign_position = new int[Num_and_Sign(char_formula)[1] + 2];//運算符號位置的陣列長度
            num = new double[sign.Length + 1];//數字的陣列長度
            Sign_Position(char_formula);
            textBox2.Text = formula;
            brackets(char_formula);
            textBox1.Text = Convert.ToString(Answer(sign,num));
            n = 0;
        }

        //判斷有幾個運算符號來指定陣列長度
        public int[] Num_and_Sign(char[] formula)
        {
            int[] count_and_position = new int[2];
            int count = 0;
            int position = 0;
            for (int i = 0; i < formula.Length; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (formula[i] == signs[j])
                    {
                        if (j < 4)
                        {
                            count++;
                        }
                        position++;
                    }
                }
            }
            count_and_position[0] = count;
            count_and_position[1] = position;
            return count_and_position;
        }

        //找出括號以及先乘除後加減，排序好每個運算的順序
        public int n = 0;
        public void brackets(char[] formula)
        {
            int a = 0;
            int b = 0;
            bool Front_bracket = false;//有沒有前括號
            for (int i = 0; i < formula.Length ; i++)
            {
                if (formula[i] == '(')//找出前括號，並存進a
                {
                    a = i;
                    Front_bracket = true;
                }
                if (formula[i] == ')' && Front_bracket == true)//找到後括號，存進b
                {
                    b = i;
                    int x = n;//紀錄這個括號的第一位是從n開始
                    for (int j = 0, m = 0; j < b; j++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            if (formula[j] == signs[k])//判斷是否是加減乘除
                            {
                                m++;//m為計算這是第幾個運算符號
                                if (j > a)//在前括號之後
                                {
                                    bool used = false;//判斷是否存過
                                    for (int l = 0; l < n ; l++)
                                    {
                                        if (sign_order[l,0] == m)//存過的不能再存
                                        {
                                            used = true;
                                        }
                                    }
                                    if (used == false)//沒有存過時
                                    {
                                        if (n > x && k > 1 )//若為乘除且不是第一位，要跟第一位的加減交換
                                        {
                                            for(int p = x; p < n; p++)
                                            {
                                                if (sign_order[p, 1] < 2)//若這一位也是乘除則跟下一位交換
                                                {
                                                    sign_order[n, 0] = sign_order[p, 0];
                                                    sign_order[n, 1] = sign_order[p, 1];
                                                    sign_order[p, 0] = m;
                                                    sign_order[p, 1] = k;
                                                    break;
                                                }
                                            }
                                            n++;
                                        }
                                        else if (n == x || k < 2)//若這是第一位或是加減運算，直接存就好
                                        {
                                            sign_order[n, 0] = m;//存進運算順序
                                            sign_order[n, 1] = k;
                                            n++;
                                        }
                                    }
                                }
                                
                            }
                        }
                    }
                    formula[a] = '^';//把括號用^取代，下次就不會抓到這個括號
                    formula[b] = '^';
                    Front_bracket = false;//處理完一個括號後，要改回false
                }
            }
            if (b != 0)
            {
                brackets(formula);
            }
            if (b == 0)
            {
                int x = n;
                for (int j = 0, m = 0; j < formula.Length; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (formula[j] == signs[k])
                        {
                            m++;
                            bool used = false;
                            for (int l = 0; l < n; l++)
                            {
                                if (sign_order[l, 0] == m)
                                {
                                    used = true;
                                }
                            }
                            if (used == false)
                            {
                                if (n > x && k > 1)
                                {
                                    for (int p = x; p < n; p++)
                                    {
                                        if (sign_order[p, 1] < 2)
                                        {
                                            sign_order[n, 0] = sign_order[p, 0];
                                            sign_order[n, 1] = sign_order[p, 1];
                                            sign_order[p, 0] = m;
                                            sign_order[p, 1] = k;
                                            break;
                                        }
                                    }
                                    n++;
                                }
                                else if (n == x || k < 2)
                                {
                                    sign_order[n, 0] = m;
                                    sign_order[n, 1] = k;
                                    n++;
                                }
                            }
                        }
                    }
                }
            }
        }

        //第幾位是運算符號，分別是什麼運算符號
        //分割數字
        public void Sign_Position(char[] formula)
        {
            string number;
            for (int i = 0, k = 0,m = 1 ; i < formula.Length; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    //第i位是運算符號時執行
                    if (formula[i] == signs[j])
                    {
                        if (j < 4)
                        {
                            sign[k] = signs[j];//將運算符號存在陣列sign
                            k++;
                        }
                        sign_position[m] = i;//判斷第幾位是符號，以便分割數字
                        m++;
                    }
                }
            }

            //設定sign_position頭尾的數字
            sign_position[0] = -1;
            sign_position[sign_position.Length - 1] = formula.Length;

            //分割數字後存進num陣列
            for (int i = 0, j = 0; i < sign_position.Length - 1; i++)
            {
                if (sign_position[i + 1] - sign_position[i] != 1)
                {
                    number = new string(formula, sign_position[i] + 1, sign_position[i + 1] - sign_position[i] - 1);//分割數字
                    num[j] = Convert.ToDouble(number);//存進陣列
                    j++;
                }
            }
        }
        
        //依照sign_order的順序執行運算
        public double Answer(char[] sign,double[] num)
        {
            //把已用過的加數減數乘數除數改為-1
            //只留下被加數被減數..等等
            for (int i = 0, j = 0; i < sign.Length; i++, j++)
            {
                int a = sign_order[i, 0] - 1;
                j = a;
                for ( ; num[j] == -1 ; j--)
                {

                }
                if (sign[a] == '+')
                {
                    num[j] = num[j] + num[a + 1];
                    num[a + 1] = -1;
                }
                if (sign[a] == '-')
                {
                    num[j] = num[j] - num[a + 1];
                    num[a + 1] = -1;
                }
                if (sign[a] == '*')
                {
                    num[j] = num[j] * num[a + 1];
                    num[a + 1] = -1;
                }
                if (sign[a] == '/')
                {
                    num[j] = num[j] / num[a + 1];
                    num[a + 1] = -1;
                }
            }
            return num[0];
        }

        private void Negative_Click(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(-Convert.ToDouble(textBox1.Text));
        }
    }
}
