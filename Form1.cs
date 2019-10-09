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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Scaner Scan = new Scaner();

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Clear();
                StreamReader sr = File.OpenText(openFileDialog1.FileName);
                //строка для считывания
                string line = null;
                //чтение первой строки
                line = sr.ReadLine();
                //чтение строк из файла и запись в textBox
                while (line != null)
                {
                    textBox1.AppendText(line);
                    textBox1.AppendText("\r");
                    line = sr.ReadLine();
                    if (line != null)
                        textBox1.AppendText("\n");
                }
                sr.Close();
            }
        }

        private void analyzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            Scan = new Scaner();
            // dataGridView3.Rows.Clear();
            Scan.Str = textBox1.Text;
            string v = null;
            Node lex;
            Scan.kar = 0;
            //Работа с магазинной памятью
            //начальное содержимое магазина - программа и маркер дна: programm = 21, маркер дна = 0 
            Scan.MP.Push(Scaner.marker);
            Scan.MP.Push(Scaner.programm);
            int temp = Scan.MP.Top();
            bool flag;
            lex = Scan.ScanStr(Scan.Str, ref Scan.kar);
            flag = Scan.MPauto(temp, lex, ref Scan.rez);
            if (!flag)
            {
                MessageBox.Show("Ошибка");
                while (!Scan.MP.IsEmpty())
                    Scan.MP.Pop();
                Scan.MP.Push(Scaner.marker);
            }
            else if (flag)
            {
                while (!Scan.MP.IsEmpty())
                    Scan.MP.Pop();
                String line = null;
                int i = 0;
                while (i < Scan.rez.commands.Size())
                {
                    int elem = Scan.rez.commands.Mas[i];
                    switch (elem)
                    {
                        case -10:
                            line = "Присвоить: " + Scan.rez.commands.Mas[i + 1] + ", " + Scan.rez.commands.Mas[i + 2] + "\r\n";
                            dataGridView2.Rows.Add(i, line);
                            line = null;
                            i += 3;
                            elem = Scan.rez.commands.Mas[i];
                            break;
                        case -1:
                            line = "Сложить: " + Scan.rez.commands.Mas[i + 1] + ", " + Scan.rez.commands.Mas[i + 2] + ", " + Scan.rez.commands.Mas[i + 3] + "\r\n";
                            dataGridView2.Rows.Add(i, line);
                            line = null;
                            i += 4;
                            elem = Scan.rez.commands.Mas[i];
                            break;
                        case -2:
                            line = "Умножить: " + Scan.rez.commands.Mas[i + 1] + ", " + Scan.rez.commands.Mas[i + 2] + ", " + Scan.rez.commands.Mas[i + 3] + "\r\n";
                            dataGridView2.Rows.Add(i, line);
                            line = null;
                            i += 4;
                            elem = Scan.rez.commands.Mas[i];
                            break;
                        case -7:
                            line = "Равно: " + Scan.rez.commands.Mas[i + 1] + ", " + Scan.rez.commands.Mas[i + 2] + ", " + Scan.rez.commands.Mas[i + 3] + "\r\n";
                            dataGridView2.Rows.Add(i, line);
                            line = null;
                            i += 4;
                            elem = Scan.rez.commands.Mas[i];
                            break;
                        case -8:
                            line = "Не равно: " + Scan.rez.commands.Mas[i + 1] + ", " + Scan.rez.commands.Mas[i + 2] + ", " + Scan.rez.commands.Mas[i + 3] + "\r\n";
                            dataGridView2.Rows.Add(i, line);
                            line = null;
                            i += 4;
                            elem = Scan.rez.commands.Mas[i];
                            break;
                        case -4:
                            line = "Метка: " + Scan.rez.commands.Mas[i + 1] + "\r\n";
                            dataGridView2.Rows.Add(i, line);
                            line = null;
                            i += 2;
                            elem = Scan.rez.commands.Mas[i];
                            break;
                        case -9:
                            line = "Переход по сравнению: " + Scan.rez.commands.Mas[i + 1] + ", " + Scan.rez.commands.Mas[i + 2] + "\r\n";
                            dataGridView2.Rows.Add(i, line);
                            line = null;
                            i += 3;
                            elem = Scan.rez.commands.Mas[i];
                            break;
                        case -5:
                            line = "Условный переход по нулю: " + Scan.rez.commands.Mas[i + 1] + ", " + Scan.rez.commands.Mas[i + 2] + "\r\n";
                            dataGridView2.Rows.Add(i, line);
                            line = null;
                            i += 3;
                            elem = Scan.rez.commands.Mas[i];
                            break;
                        case -6:
                            line = "Безусловный переход: " + Scan.rez.commands.Mas[i + 1] + "\r\n";
                            dataGridView2.Rows.Add(i, line);
                            line = null;
                            i += 2;
                            elem = Scan.rez.commands.Mas[i];
                            break;
                        case -3:
                            line = "Возведение в степень: " + Scan.rez.commands.Mas[i + 1] + ", "  + Scan.rez.commands.Mas[i + 2] + ", "  + Scan.rez.commands.Mas[i + 3] + "\r\n";
                            dataGridView2.Rows.Add(i, line);
                            line = null;
                            i += 4;
                            elem = Scan.rez.commands.Mas[i];
                            break;

                        default:
                            //dataGridView3.Rows.Add(i, line);
                            break;
                    }
                }
                int j = 0;
                while (j < Scan.rez.data.Size())
                {
                    dataGridView1.Rows.Add(j, Scan.rez.data.Mas[j].name, Scan.rez.data.Mas[j].value);
                    j++;
                }

                int k = 0;
                while (k < Scan.rez.labels.Size())
                {
                    dataGridView3.Rows.Add(k, Scan.rez.labels.Mas[k]);
                    k++;
                }

            }
        }

        private void трансляторToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void интерпретироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView3.Rows.Clear();
            Scan.Interpretator(ref Scan.rez);
            int i = 0;
            while (i < Scan.rez.data.Size())
            {
                dataGridView1.Rows.Add(i, Scan.rez.data.Mas[i].name, Scan.rez.data.Mas[i].value);
                i++;
            }
            int k = 0;
            while (k < Scan.rez.labels.Size())
            {
                dataGridView3.Rows.Add(k, Scan.rez.labels.Mas[k]);
                k++;
            }
        }
    }
}