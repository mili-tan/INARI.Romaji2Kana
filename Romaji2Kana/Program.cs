using System;
using IniParser.Model;
using System.IO;
using System.Text;
using System.Windows.Forms;
using IniParser;
using Microsoft.International.Converters;

namespace Romaji2Kana
{
    static class Program
    {
        public static IniData UstData;
        private static readonly Encoding EncodeJPN = Encoding.GetEncoding("Shift_JIS");
        private static readonly string UstHeader = "[#VERSION]\r\n" + "UST Version 1.20\r\n";

        static void Main(string[] path)
        {
            if (!string.IsNullOrWhiteSpace(string.Join("", path)))
            {
                string ustFileStr = File.ReadAllText(string.Join("", path))
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
                        itemSection.Keys["Lyric"] = KanaConverter.RomajiToHiragana(itemSection.Keys["Lyric"]);
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
                MessageBox.Show("未包含应有的参数，请作为UTAU插件使用");
            }
        }
    }
}
