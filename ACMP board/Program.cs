using System;
using System.Collections.Generic;
using System.Linq;

namespace ACMP_board
{
    class Program
    {
        static void Main(string[] args)
        {
            UI.PrintButtons();

            MyKeys lastKey = MyKeys.List;
            MyKeys currentKey = Converter.ConsoleKeyToMyKey(Console.ReadKey().Key);

            do
            {
                bool makeReset = false;

                switch (currentKey)
                {
                    case MyKeys.List:
                        Console.Clear();
                        UI.PrintList();
                        break;

                    case MyKeys.Reset:
                        makeReset = true;
                        currentKey = lastKey;
                        break;

                    case MyKeys.SearchForVideos:
                        Console.Clear();
                        UI.PrintVideos();
                        break;

                    case MyKeys.ListWithVideo:
                        Console.Clear();
                        UI.PrintList(true);
                        break;

                    case MyKeys.Close:
                        return;

                    default:
                        currentKey = MyKeys.None;
                        break;
                }

                lastKey = currentKey == MyKeys.None ? lastKey : currentKey;

                if (currentKey != MyKeys.None)
                    UI.PrintButtons();

                if (!makeReset)
                    currentKey = Converter.ConsoleKeyToMyKey(Console.ReadKey().Key);

            } while (true);
        }
    }
}
