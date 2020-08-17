using System;
using System.Collections.Generic;
using System.Text;

namespace ACMP_board
{
    public enum MyKeys
    {
        None = 0,
        List = ConsoleKey.L,
        Reset = ConsoleKey.F5,
        SearchForVideos = ConsoleKey.S,
        ListWithVideo = ConsoleKey.D,
        Close = ConsoleKey.Escape,
    }

    public static class Converter
    {
        public static MyKeys ConsoleKeyToMyKey(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.L:
                    return MyKeys.List;

                case ConsoleKey.F5:
                    return MyKeys.Reset;

                case ConsoleKey.S:
                    return MyKeys.SearchForVideos;

                case ConsoleKey.D:
                    return MyKeys.ListWithVideo;

                case ConsoleKey.Escape:
                    return MyKeys.Close;

                default:
                    return MyKeys.None;
            }
        }
    }
}