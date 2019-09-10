using System;
using Nancy;
using Nancy.Hosting.Self;

namespace FoxyListener
{
    static class Program
    {
        static void Main()
        {
            Console.WriteLine("Foxy");
            Console.WriteLine("-------------------");

            using (NancyHost host = new NancyHost(new HostConfiguration
                    { RewriteLocalhost = true, UrlReservations = new UrlReservations { CreateAutomatically = true } },
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
            Get("/", x => "OK");
        }
    }
}