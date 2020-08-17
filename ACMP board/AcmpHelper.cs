using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Linq;
using System.IO;
using System.Net;

namespace ACMP_board
{
    public static class AcmpHelper
    {
        public static readonly string path = "https://acmp.ru/index.asp";
           public static readonly string tableCsvUrl =
            "https://docs.google.com/spreadsheets/d/1dpvkMqneA3e664Ogx_zY-xuTNIK8LyWrLYSD5Z3tTDY/export?format=csv&id=1dpvkMqneA3e664Ogx_zY-xuTNIK8LyWrLYSD5Z3tTDY&gid=0";

                             public static readonly string youtubeUrl = "https://www.youtube.com/results?search_query=";

                
                  public static Encoding encoding = Encoding.UTF8;
         
        public static HtmlNode GetBoardNode(HtmlDocument doc)
        {
            HtmlNode node;
            HtmlNode prev;

            node = doc.DocumentNode;
            node = node.ChildNodes.FindFirst("body");
            node = node.ChildNodes.FindFirst("table");

            prev = node;

            node = prev.ChildNodes.FindFirst("tr");
            node.Remove();

            node = prev.ChildNodes.FindFirst("tr");
            node.Remove();

            node = prev.ChildNodes.FindFirst("tr");


            node = node.ChildNodes.FindFirst("td");
            node = node.ChildNodes.FindFirst("table");
            node = node.ChildNodes.FindFirst("tr");

            prev = node;

            node = prev.ChildNodes.FindFirst("td");
            node.Remove();

            node = prev.ChildNodes.FindFirst("td");


            node = node.ChildNodes.FindFirst("table");

            prev = node;

            node = prev.ChildNodes.FindFirst("tr");
            node.Remove();
            node = prev.ChildNodes.FindFirst("tr");
            node.Remove();

            return prev;
        }

        public static List<AcmpHelper.Task> GetTable(HtmlNode table)
        {
            List<AcmpHelper.Task> resList = new List<Task>();
            HtmlNode node = table.ChildNodes.FindFirst("tr");

            while (node != null)
            {
                HtmlNode cur;

                Task t = new Task();

                cur = node.ChildNodes.FindFirst("td");
                t.SId = cur.InnerText;
                cur.Remove();

                cur = node.ChildNodes.FindFirst("td");
                t.Name = ParseString(cur.InnerText);
                cur.Remove();

                cur = node.ChildNodes.FindFirst("td");
                t.Theme = ParseString(cur.InnerText);
                cur.Remove();

                cur = node.ChildNodes.FindFirst("td");
                t.HasAnalysis = (ParseString(cur.InnerText) == "Да");
                cur.Remove();


                cur = node.ChildNodes.FindFirst("td");
                t.Complexity = ParsePercents(cur.InnerText);
                cur.Remove();

                resList.Add(t);

                node.Name += "Checked";
                node = table.ChildNodes.FindFirst("tr");
            }
            return resList;
        }
        public static List<AcmpHelper.Task> GetTable(HtmlNode table, List<int> tasks)
        {
            List<AcmpHelper.Task> resList = new List<Task>();
            HtmlNode node = table.ChildNodes.FindFirst("tr");

            while (node != null)
            {
                HtmlNode cur;
                Task t = new Task();


                cur = node.ChildNodes.FindFirst("td");
                t.SId = cur.InnerText;
                cur.Remove();

                node.Name += "Checked";
                if (!tasks.Contains(t.ID))
                {
                    node = table.ChildNodes.FindFirst("tr");
                    continue;

















                }

                cur = node.ChildNodes.FindFirst("td");
                t.Name = ParseString(cur.InnerText);
                cur.Remove();

                cur = node.ChildNodes.FindFirst("td");
                t.Theme = ParseString(cur.InnerText);
                cur.Remove();

                cur = node.ChildNodes.FindFirst("td");
                t.HasAnalysis = (ParseString(cur.InnerText) == "Да");
                cur.Remove();


                cur = node.ChildNodes.FindFirst("td");
                t.Complexity = ParsePercents(cur.InnerText);
                cur.Remove();

                resList.Add(t);

                node = table.ChildNodes.FindFirst("tr");

            }
            return resList;
        }

        public static List<AcmpHelper.Task> GetTasks(List<int> tasks)
        {
            List<Task> result = new List<Task>();

            tasks.Sort();

            for (int i = 0; i < 22 && tasks.Count != result.Count; ++i)
            {
                List<int> cur = tasks.Where(t => t <= (i + 1) * 50).Where(t => t > i * 50).ToList();
                if (cur.Count != 0)
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(path + $"?main=tasks&page={i}&id_type=0");
                    Console.WriteLine("GET request from\t" + path + $"?main=tasks&page={i}&id_type=0");
                    req.Method = "GET";
                    HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    Console.WriteLine(resp.StatusDescription);
                    HtmlDocument doc = new HtmlDocument();
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(1251)))
                    {
                        doc.Load(reader);
                    }

                    var board = GetBoardNode(doc);
                    var table = GetTable(board, cur);
                    result.AddRange(table);
                }
            }

            return result;
        }

        public static List<int> GetNumsOfTasks(char col)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(tableCsvUrl);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
            string excel = sr.ReadToEnd();
            sr.Close();

            int start = 9;
            int finish = start + 50;
            int column = col - 65;


            List<List<string>> table = excel.Split("\n").Select(p => p.Split(",").ToList()).ToList();

            if (table.Count == 0)
                throw new Exception("Table is empty");
            if (table[0].Count <= column)
                throw new Exception($"There is no column{column}");

            List<int> result = new List<int>();

            for (int i = start - 1; i < finish; ++i)
                if (table[i][column] != string.Empty)
                    result.Add(int.Parse(table[i][column]));
                else
                    break;
            return result;
        }

        public static bool HasVideo(int taskNum)
        {
            string reqString = youtubeUrl + $"{taskNum}+acmp.ru";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(reqString);
            req.Method = "GET";

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            bool result = false;

            using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                result = reader.ReadToEnd().Contains($"Разбор задачи {taskNum} acmp.ru");

            return result;
        }

        public static double ParsePercents(string str)
        {
            string res = "";

            foreach (char c in str)
                if (c >= 48 && c <= 57)
                    res += c;

            return double.Parse(res) / 100.0;
        }

        public static string ParseString(string str)
        {
            string res = "";

            foreach (char c in str)
                if (c != '\n' && c != '\t' && c != '\r')
                    res += c;
            return res;
        }

        public class Task
        {
            public int ID => int.Parse(SId);
            public string SId;
            public string Name;
            public string Theme;
            public bool HasAnalysis;
            public double Complexity;

            public bool HasVideo;
        }
    }
}