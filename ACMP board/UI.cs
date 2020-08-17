using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ACMP_board
{
    public static class UI
    {

        public static void PrintList(bool withVideo = false)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Loading data:");

                Console.WriteLine("Getting data from CSV file");

                List<int> tasks = AcmpHelper.GetNumsOfTasks('H');
                //List<int> tasks = new List<int>(){1, 110, 112, 116};

                Console.WriteLine("OK");

                Console.WriteLine("Connecting to ACMP:");
                List<AcmpHelper.Task> ToPrintTasks = AcmpHelper.GetTasks(tasks);

                Console.Clear();

                Console.Write($"{"Номер",7}|");
                Console.Write($"{"Название",40}|");
                Console.Write($"{"Тема",40}|");
                Console.Write($"{"Разбор",10}|");
                if (withVideo)
                    Console.Write($"{"Видео",10}|");
                Console.WriteLine($"{"Сложность",10}");

                ToPrintTasks = ToPrintTasks.OrderBy(t => t.Complexity).ToList();

                foreach (AcmpHelper.Task t in ToPrintTasks)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{t.SId,7}|");

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{t.Name,40}|");

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write($"{t.Theme,40}|");

                    Console.ForegroundColor = (t.HasAnalysis ? ConsoleColor.Green : ConsoleColor.Red);
                    Console.Write($"{(t.HasAnalysis ? "Да" : "Нет"),10}|");

                    if (withVideo)
                    {
                        t.HasVideo = AcmpHelper.HasVideo(t.ID);

                        Console.ForegroundColor = (t.HasVideo ? ConsoleColor.Green : ConsoleColor.Red);
                        Console.Write($"{(t.HasVideo ? "Да" : "Нет"),10}|");
                    }

                    if (t.Complexity < 1)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    if (t.Complexity < 0.9)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    if (t.Complexity < 0.8)
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    if (t.Complexity < 0.7)
                        Console.ForegroundColor = ConsoleColor.Red;
                    if (t.Complexity < 0.6)
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    if (t.Complexity < 0.5)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    if (t.Complexity < 0.4)
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    if (t.Complexity < 0.3)
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (t.Complexity < 0.2)
                        Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine($"{Math.Round(t.Complexity * 100) + "%",10}");
                }
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void PrintVideos()
        {
            List<int> tasks = AcmpHelper.GetNumsOfTasks('H');
            Queue<int> hasVideos = new Queue<int>();

            for (int i = 0; i < tasks.Count; ++i)
            {
                int task = tasks[i];
                if (AcmpHelper.HasVideo(task))
                    hasVideos.Enqueue(task);
                Console.Clear();
                drawTextProgressBar(i, tasks.Count);
            }

            Console.Clear();
            if (hasVideos.Count != 0)
            {
                Console.WriteLine("Next tasks has video analysis from '3.5 задачи в неделю'");
                foreach (int i in hasVideos)
                {
                    Console.WriteLine(i);
                }
            }
            else
            {
                Console.WriteLine("Video analysis are over");
            }
        }

        public static void PrintButtons()
        {
            Console.WriteLine();
            Console.Write("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine();

            PrintButton("Reset", ConsoleKey.F5);
            PrintButton("Close", ConsoleKey.Escape);
            PrintButton("Watch list of your tasks", ConsoleKey.L);
            PrintButton("Watch list of your tasks, with info about videos", ConsoleKey.D);
            PrintButton("Watch list of your tasks, that have video analysis", ConsoleKey.S);
        }

        public static void PrintButton(string message, ConsoleKey button)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{message,60} : ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(button.ToString());

            Console.ResetColor();
        }

        private static void drawTextProgressBar(int progress, int total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write("#");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write("-");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess
        }
    }
}