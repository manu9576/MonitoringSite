using MonitoringSite;
using System;

namespace ConsoleMonitoring
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            SiteParameters googleDNS = new SiteParameters("8.8.8.8");

            Console.WriteLine("Test 8.8.8.8 : " + Worker.PingSite(googleDNS,1));

            Console.ReadKey();
        }
    }
}
