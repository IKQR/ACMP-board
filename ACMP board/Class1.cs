using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using System.Net;

namespace ACMP_board
{
    public static class Class1
    {
                             public static readonly string path = "https://acmp.ru/index.asp";
        public static readonly string tableCsvUrl =
                            "https://docs.google" +
                            ".com/spreadsheets/d/1dpvkMqneA3e664Ogx_zY-xuTNIK8LyWrLYSD5Z3tTDY/export?format=csv&id=1dpvkMqneA3e664Ogx_zY-xuTNIK8LyWrLYSD5Z3tTDY&gid=0";

                    public static readonly string youtubeUrl = "https://www.youtube.com/results?search_query=";

                       public static Encoding encoding = Encoding.UTF8;
    }
}