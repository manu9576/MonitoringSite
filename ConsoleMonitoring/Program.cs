using MonitoringSite;
using System;

namespace ConsoleMonitoring
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("Test 8.8.8.8 : " + Worker.PingSite("8.8.8.8",500));

            Console.ReadKey();
        }
    }
}
