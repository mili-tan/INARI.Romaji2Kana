﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;

namespace Kana2Romaji
{
    static class Program
    {
        public static IniData UstData;
        private static Dictionary<string,string> RmDictionary = new Dictionary<string, string>();
        private static readonly Encoding EncodeJPN = Encoding.GetEncoding("Shift_JIS");
        private static readonly string UstHeader = "[#VERSION]\r\n" + "UST Version 1.20\r\n";
        static void Main(string[] path)
        {
            foreach (var item in Resource.Table.Split(new[] {'\n'},StringSplitOptions.RemoveEmptyEntries))
                RmDictionary.Add(item.Split(',')[0], item.Split(',')[1]);

            if (!string.IsNullOrWhiteSpace(string.Join("", path)))
            {
                string ustFileStr = File.ReadAllText(string.Join("", path), EncodeJPN)
                    .Replace(UstHeader, "");

                UstData = new FileIniDataParser().Parser.Parse(ustFileStr);

                UstData.Sections.RemoveSection("#PREV");
                UstData.Sections.RemoveSection("#NEXT");
                UstData.Sections.RemoveSection("#SETTING");

                foreach (var itemSection in UstData.Sections)
                {
                    if (itemSection.Keys["Lyric"] == "R") continue;
                    try
                    {
                        if (itemSection.Keys["Lyric"].Contains(" "))
                            itemSection.Keys["Lyric"] = itemSection.Keys["Lyric"].Trim().Split(' ')[0] + " " +
                                                        RmDictionary[itemSection.Keys["Lyric"].Trim().Split(' ')[1]];
                        else
                            itemSection.Keys["Lyric"] = RmDictionary[itemSection.Keys["Lyric"].Trim()];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                File.WriteAllText(string.Join("", path),
                    UstHeader + UstData.ToString().Replace(" = ", "=").Replace("\r\n\r\n", "\r\n"), EncodeJPN);
            }
            else
            {
                MessageBox.Show(@"未包含应有的参数，请作为UTAU插件使用");
            }
        }
    }
}