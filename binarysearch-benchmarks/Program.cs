using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binarysearch_benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var numberList = new int[] { 2, 6, 8, 12, 15, 16, 22, 44, 74 };
            var key = 15;
            BenchmarkIterative(numberList, key);

            Console.ReadKey();
        }

        static object IterativeSearch(int[] list, int key)
        {
            var min = 0;
            var max = list.Length - 1;

            while (min <= max)
            {
                var mid = (min + max) / 2;

                if (list[mid] == key)
                {
                    return mid;
                }

                else if (key < list[mid])
                {
                    max = mid - 1;
                }

                else
                {
                    min = mid + 1;
                }
            }



            return "Not Found";
        }


        static void BenchmarkIterative(int[] numberList, int key)
        {
            var stopwatch = new Stopwatch();
            var n = 1000000;
            var ticks = 0d;

            for (int i = 0; i < n; i++)
            {
                stopwatch.Reset();
                stopwatch.Start();               
                IterativeSearch(numberList, key);
                ticks += stopwatch.ElapsedTicks;
            };
            
            Console.WriteLine("Benchmarking Iterative Search");
            Console.WriteLine("*****************************");
            double nanoseconds = (ticks / Stopwatch.Frequency) * 1000000000;
            Console.WriteLine("{0:N2} ns: ", nanoseconds / n);          
        }
    }
}
