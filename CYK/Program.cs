using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
 

namespace CYK_CONS
{
    class Program
    {
        static void Main(string[] args)
        {
            string s;
            string s_2;
            string path_in_gr_file; // путь к исход грамматике
            string path_in_test_file; //путь к тесту
            StreamReader f_show;// поток для отображения данных
            StreamReader f2;// поток для преобразования в формат, который я буду анализировать
            StreamReader f1;//
            string[,] st = new string[0, 0];
            List<string>[,] table = new List<string>[0, 0];
            Console.WriteLine("Введите имя файла, в котором задана грамматика ");
            path_in_gr_file = Console.ReadLine();
                if(path_in_gr_file=="")
                {
                    f_show = File.OpenText("g1.txt");
                     f2 = File.OpenText("g1.txt");
                }
                else
                {
                   f_show = File.OpenText(path_in_gr_file);
                   f2 = File.OpenText(path_in_gr_file);
                }
             
            Console.WriteLine("Введите имя файла, в котором задана проверяемая строка ");
            path_in_test_file = Console.ReadLine();
            if (path_in_test_file == "")
            {
                f1 = File.OpenText("1.txt");
            }
            else
            {
                f1 = File.OpenText(path_in_test_file);
            }
            s = f1.ReadToEnd();
            s_2 = f_show.ReadToEnd();
            Readfromfile(out st, new string[3] { "=", "->", "=>" }, f2);
            Console.WriteLine();
            Console.WriteLine("Цепочка символов, ");
            Console.WriteLine(s);
            Console.WriteLine();
            Console.WriteLine("Грамматика, ");
            Console.WriteLine(s_2);
           // Output1(st);
            TriangleTable(out table, s, st);
            Console.WriteLine();
            if (table[0, 0].Contains(st[0, 0]))
            {
                Console.WriteLine("Цепочка символов {0} принадлежит языку," +
                       " заданному данной грамматикой, ", s);
            }
            else
            {
                Console.WriteLine("Цепочка символов {0} не принадлежит языку," +
                       " заданному данной грамматикой, ", s);
            }
            Console.WriteLine();
            Console.WriteLine("Таблица,");
            Console.WriteLine();
            OutputTable(table);

            f1.Close();
            f2.Close();
            Console.ReadKey(true);
        }


        static void TriangleTable(out List<string>[,] table, string s, string[,] rule)
        {
            int n;
            string s1;
            List<string> s2 = new List<string>();

            table = new List<string>[s.Length, s.Length];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    table[i, j] = new List<string>();
                }
            }
            n = s.Length - 1;
            /* Построение последней строки таблицы, */
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j < rule.GetLength(0); j++)
                {
                    if (string.Equals(s[i].ToString(), rule[j, 1]))
                    {
                        if (!table[n, i].Contains(rule[j, 0]))
                        {
                            table[n, i].Add(rule[j, 0]);

                        }
                    }
                }
            }
            /* Построение предпоследней строки таблицы, */
            if (n > 0)
            {
                for (int j = 0; j < n; j++)
                {

                    for (int i1 = 0; i1 < table[n, j].Count; i1++)
                    {
                        for (int j1 = 0; j1 < table[n, j + 1].Count; j1++)
                        {
                            s1 = table[n, j][i1] + table[n, j + 1][j1];

                            for (int k = 0; k < rule.GetLength(0); k++)
                            {
                                if (string.Equals(s1, rule[k, 1]))
                                {
                                    if (!table[n - 1, j].Contains(rule[k, 0]))
                                    {
                                        table[n - 1, j].Add(rule[k, 0]);
                                    }
                                }
                            }
                        }
                    }
                }

                /* Построение всех остальных строк,  
                 * t[i,j] строится на основе конкатенаций  t[i+2,j]*t[i+1,j+1] и  t[i+1,j]*t[i+2,j+2]*/

                if (n > 1)
                {
                    for (int i = n - 2; i >= 0; i--)
                    {
                        for (int j = 0; j < i + 1; j++)
                        {
                            /* Составляем множество слов, */
                            s2 = new List<string>();
                            for (int u = 0; u < n - i; u++)
                            {
                                for (int i1 = 0; i1 < table[n - u, j].Count; i1++)
                                {
                                    for (int j1 = 0; j1 < table[i + u + 1, j + u + 1].Count; j1++)
                                    {
                                        s1 = table[n - u, j][i1] + table[i + u + 1, j + u + 1][j1];
                                        if (!s2.Contains(s1))
                                        {
                                            s2.Add(s1);
                                        }
                                    }
                                }
                            }

                            /* проверяем для каждогослова из множества,
                                из какого правила оно выводится, */

                            for (int i1 = 0; i1 < s2.Count; i1++)
                            {
                                for (int k = 0; k < rule.GetLength(0); k++)
                                {
                                    if (string.Equals(s2[i1], rule[k, 1]))
                                    {
                                        if (!table[i, j].Contains(rule[k, 0]))
                                        {
                                            table[i, j].Add(rule[k, 0]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }




        /// <summary>
        ///    Чтение грамматики из файла,
        /// </summary>
        /// <param name="rules">массив непустых строк,</param>
        /// <param name="q">срока - разделитель левой и правой части,</param>
        /// <param name="f">файл, </param>
        static void Readfromfile(out string[,] rules, string[] q, StreamReader f)
        {
            int n;
            string s;
            int index = 0;
            int index1 = 0;
            int u = 0;
            string s1, s2;
            int m;
            Dictionary<int, string> p = new Dictionary<int, string>();
            Dictionary<int, string> p1 = new Dictionary<int, string>();
            n = 0;
            while ((s = f.ReadLine()) != null)
            {
                s.Trim();
                if (!string.IsNullOrWhiteSpace(s))
                {
                    p[n] = s.Trim();
                    n++;
                }
            }

            m = 0;
            for (int i = 0; i < p.Count; i++)
            {
                for (int j = 0; j < q.Length; j++)
                {
                    if (p[i].IndexOf(q[j]) > -1)
                    {
                        index = p[i].IndexOf(q[j]);
                        index1 = j;
                        break;
                    }
                }
                s1 = p[i].Substring(index + q[index1].Length).Trim();
                while (s1.IndexOf("|") > -1)
                {
                    u = s1.IndexOf("|");
                    s2 = s1.Substring(0, u).Trim();
                    s1 = s1.Substring(u + 1).Trim();
                    p1[m] = string.Format("{0} = {1}", p[i].Substring(0, index).Trim(), s2);
                    m++;
                }
                p1[m] = string.Format("{0} = {1}", p[i].Substring(0, index).Trim(), s1);
                m++;

            }
            rules = new string[m, 2];
            for (int i = 0; i < rules.GetLength(0); i++)
            {
                index = p1[i].IndexOf("=");
                rules[i, 0] = p1[i].Substring(0, index).Trim();
                rules[i, 1] = p1[i].Substring(index + 1).Trim();
            }
        }

        static void Output1(string[,] rules)
        {
            for (int i = 0; i < rules.GetLength(0); i++)
            {
                Console.WriteLine("{0} = {1}", rules[i, 0], rules[i, 1]);

            }
        }

        static void OutputTable(List<string>[,] table)
        {

            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    if (table[i, j].Count > 0)
                    {
                        Console.Write("{ ");
                        for (int k = 0; k < table[i, j].Count; k++)
                        {
                            Console.Write("{0}, ", table[i, j][k]);
                        }
                        Console.Write(" }");
                    }
                    else
                    {
                        Console.Write("{ 0 }");
                    }
                }
                Console.WriteLine();
            }
        }



        
    }
}
