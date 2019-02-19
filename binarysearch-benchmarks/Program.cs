using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace binarysearch_benchmarks
{
    public class Program
    {
        private static double result;
        private static readonly int[] list = { 2, 6, 8, 12, 15, 16, 22, 44, 74 };
        private static readonly int key = 15;

        public static int NumberOfTimesCalled = 0;
        static void Main(string[] args)
        {
           var iterations = 1000000000;
           Benchmark("Iterative", iterations, IterativeSearch);
           Benchmark("Recursive", iterations, RecursiveSearch);
           Console.ReadKey();

           // If the compiler has tail-call optimization then the iterative and recursive approach
           // will be equally fast. The .NET/C# compiler does NOT so the iterative is slightly faster.
        }

        static double IterativeSearch(int[] list, int key)
        {
            var min = 0;
            var max = list.Length - 1;

            while (min <= max)
            {
                var mid = (min + max) / 2;

                if (list[mid] == key)
                {
                    return ++mid;
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

            //return "Not Found";
            return 0;
        }

        static double RecursiveSearch(int[] array, int NumberToSearchFor, int min, int max)
        {
            NumberOfTimesCalled++;
            if (min > max)
            {
                return 0;
                //return "Nil";
            }
            else
            {
                int mid = (min + max) / 2;
                if (NumberToSearchFor == array[mid])
                {
                    return ++mid;
                }
                else if (NumberToSearchFor < array[mid])
                {
                    return RecursiveSearch(array, NumberToSearchFor, min, mid - 1);
                }
                else
                {
                    return RecursiveSearch(array, NumberToSearchFor, mid + 1, max);
                }
            }
        }

        static void Benchmark(string description, int iterations, Func<int[], int, double> method)
        {
            // eliminating overhead by GC by ensuring it has run before we start
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();


            // warm up
            // calling the function outside the timing loop so the overhead of the .NET JIT compiler isn't included
            var temp = method(list, key);

            // using stopwatch instead of DateTime.Now for a more accurate result
            var watch = new Stopwatch();
            watch.Start();

            // unrolling the loop i.e doing multiple calls in the loop minimize the impact of the for loop computations
            var loops = iterations / 10;
            for (int i = 0; i < loops; i++)
            {
                // deadcode elimination
                // store the result in a class variable to prevent the JIT compiler from optimizing it
                result = method(list, key);
                result = method(list, key);
                result = method(list, key);
                result = method(list, key);
                result = method(list, key);
                result = method(list, key);
                result = method(list, key);
                result = method(list, key);
                result = method(list, key);
                result = method(list, key);
            }
            watch.Stop();
            Console.WriteLine("Benchmarking..." + description);
            Console.WriteLine(
                "{0:0.00} ms ({1:N0} ticks) (over {2:N0} iterations), {3:N0} ops/milliseconds",
                watch.ElapsedMilliseconds, watch.ElapsedTicks, iterations,
                (double)iterations / watch.ElapsedMilliseconds);
        }

        static void Benchmark(string description, int iterations, Func<int[], int, int, int, double> method)
        {
            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();


            // warm up
            var temp = method(list, key, 0, 0);

            var watch = new Stopwatch();
            watch.Start();
            var loops = iterations / 10;
            for (int i = 0; i < loops; i++)
            {
                result = method(list, key, 0, 0);
                result = method(list, key, 0, 0);
                result = method(list, key, 0, 0);
                result = method(list, key, 0, 0);
                result = method(list, key, 0, 0);
                result = method(list, key, 0, 0);
                result = method(list, key, 0, 0);
                result = method(list, key, 0, 0);
                result = method(list, key, 0, 0);
                result = method(list, key, 0, 0);
            }
            watch.Stop();
            Console.WriteLine("Benchmarking... " + description);
            Console.WriteLine(
                "{0:0.00} ms ({1:N0} ticks) (over {2:N0} iterations), {3:N0} ops/milliseconds",
                watch.ElapsedMilliseconds, watch.ElapsedTicks, iterations,
                (double)iterations / watch.ElapsedMilliseconds);
        }
    }
}
