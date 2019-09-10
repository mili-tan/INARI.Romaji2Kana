using System;
using System.IO;
using System.Linq;
using System.Text;
using IniParser;
using IniParser.Model;
using Nancy;
using Nancy.Hosting.Self;
using static FoxyListener.Program;

namespace FoxyListener
{
    static class Program
    {
        public static IniData UstData;
        public static string UstFileStr;
        private static readonly Encoding EncodeJPN = Encoding.GetEncoding("Shift_JIS");
        private static readonly string UstHeader = "[#VERSION]\r\n" + "UST Version 1.20\r\n";

        static void Main(string[] path)
        {
            UstFileStr = File.ReadAllText(string.Join("", path), EncodeJPN)
                .Replace(UstHeader, "");
            UstData = new FileIniDataParser().Parser.Parse(UstFileStr);

            UstData.Sections.RemoveSection("#PREV");
            UstData.Sections.RemoveSection("#NEXT");
            UstData.Sections.RemoveSection("#SETTING");

            using (NancyHost host = new NancyHost(new HostConfiguration
                    {RewriteLocalhost = true, UrlReservations = new UrlReservations {CreateAutomatically = true}},
                new Uri("http://localhost:2020/")))
            {
                host.Start();

                Console.WriteLine("Foxy is running on 2020 port");
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
            }
        }
    }

    public sealed class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", x => "Welcome to Foxy");
            Get("/get", x => UstData.ToString());
            Get("/get/count", x => UstData.Sections.Count.ToString());
            Get("/get/full", x => UstFileStr);
            Get("/get/{section}/{key}", x =>
            {
                try
                {
                    return new FileIniDataParser().Parser.Parse(UstFileStr)
                        .Sections["#" + x.section][x.key].ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return e.ToString();
                }
            });
        }
    }
}
