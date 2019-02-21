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
        private static readonly Random rnd = new Random();

        public static int NumberOfTimesCalled = 0;
        static void Main(string[] args)
        {
            SystemInfo();

            //Console.WriteLine("{0,-25} {1}{2,15:F1} ns {3,10:F2} {4,10:D}", "Message", "Info", "mean", "sdev", "count");

            Console.WriteLine("Benchmarking the iterative search... \n");
            SetUpAndBench("Iterative binary search", IterativeSearch);

            Console.WriteLine("Benchmarking the recursive search... \n");
            SetUpAndBench("Recursive binary search", RecursiveSearch);

            // If the compiler has tail-call optimization then the iterative and recursive approach
            // will be equally fast. The .NET/C# compiler does NOT so the iterative is slightly faster.
        }

        private static void SetUpAndBench(string description, Func<int, int[], double> method)
        {

            // eliminating overhead by GC by ensuring it has run before we start
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
       
            // warm up by calling the function outside the timing loop so the overhead of the .NET JIT compiler isn't included
            var temp = method(15, Enumerable.Range(0, 100).ToArray());

            Benchmark(description, method);
        }

        private static void SetUpAndBench(string description, Func<int[], int, int, int, double> method)
        {

            // eliminating overhead by GC by ensuring it has run before we start
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            // warm up by calling the function outside the timing loop so the overhead of the .NET JIT compiler isn't included
            var temp = method(Enumerable.Range(0, 100).ToArray(), 15, 0 ,0);

            Benchmark(description, method);
        }

        private static void Benchmark(string description, Func<int, int[], double> method)
        {
            for (int size = 100; size <= 10_000_000; size *= 2)
            {
                int[] intArr = ArrayHelpers.FillIntArray(size);
                int[] items = ArrayHelpers.FillIntArray(size);
                int n = size;
                ArrayHelpers.Shuffle(items);

                RunBench(description,
                string.Format("{0,8:D}", size),
                  i => method(items[i % n], intArr));
            }
        }

        private static void Benchmark(string description, Func<int[], int, int, int, double> method)
        {
            for (int size = 100; size <= 10_000_000; size *= 2)
            {
                int[] intArr = ArrayHelpers.FillIntArray(size);
                int[] items = ArrayHelpers.FillIntArray(size);
                int n = size;
                ArrayHelpers.Shuffle(items);

                RunBench(description,
                string.Format("{0,8:D}", size),
                  i => method(intArr, items[i % n], 0, n));
            }
        }

        public static double RunBench(string msg, string info, Func<int, double> f,
                             int n = 10, double minTime = 0.25)
        {
            int count = 1, totalCount = 0;
            double dummy = 0.0, runningTime = 0.0, st = 0.0, sst = 0.0;
            do
            {
                count *= 2;
                st = sst = 0.0;
                for (int j = 0; j < n; j++)
                {
                    Timer t = new Timer();
                    for (int i = 0; i < count; i++)
                        dummy += f(i);
                    runningTime = t.Check();
                    double time = runningTime * 1e9 / count;
                    st += time;
                    sst += time * time;
                    totalCount += count;
                }
            } while (runningTime < minTime && count < Int32.MaxValue / 2);
            double mean = st / n, sdev = Math.Sqrt((sst - mean * mean * n) / (n - 1));
            Console.WriteLine("{0,-25} {1}{2,15:F1} ns {3,10:F2} {4,10:D}", msg, info, mean, sdev, count);
            return dummy / totalCount;
        }



        static double IterativeSearch(int key, int[] list)
        {
            int n = list.Length, a = 0, b = n - 1;
            while (a <= b)
            {
                int i = (a + b) / 2;
                if (key < list[i])
                    b = i - 1;
                else if (list[i] < key)
                    a = i + 1;
                else
                    return i;
            }
            return -1;
        }

        static double RecursiveSearch(int[] array, int key, int min, int max)
        {
            if (min > max)
            {
                return -1;
            }

            int mid = ((max - min) / 2) + min;

            if (key > array[mid])
            {
                return RecursiveSearch(array, key, mid + 1, max);
            }
            else if (key < array[mid])
            {
                return RecursiveSearch(array, key, min, mid - 1);
            }
            else
            {
                return mid;
            }
        }




        public static String Get45PlusFromRegistry()
        {
            const string subkey = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\";
            using (var ndpKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry32).OpenSubKey(subkey))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                    return CheckFor45PlusVersion((int)ndpKey.GetValue("Release"));
                else
                    return null;
            }
        }

        // Checking the version using >= will enable forward compatibility.
        private static string CheckFor45PlusVersion(int releaseKey)
        {
            if (releaseKey >= 394802) return "4.6.2 or later";
            if (releaseKey >= 394254) return "4.6.1";
            if (releaseKey >= 393295) return "4.6";
            if (releaseKey >= 379893) return "4.5.2";
            if (releaseKey >= 378675) return "4.5.1";
            if (releaseKey >= 378389) return "4.5";
            return null;
        }

        private static void SystemInfo()
        {
            Console.WriteLine("# OS          {0}",
              Environment.OSVersion.VersionString);
            Console.WriteLine("# .NET vers.  {0}",
              //      Environment.Version); // Non-Windows or .Net < 4.5
              Get45PlusFromRegistry());     // Windows and .Net >= 4.5
            Console.WriteLine("# 64-bit OS   {0}",
              Environment.Is64BitOperatingSystem);
            Console.WriteLine("# 64-bit proc {0}",
              Environment.Is64BitProcess);
            Console.WriteLine("# CPU         {0}; {1} \"cores\"",
              Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER"),
              Environment.ProcessorCount);
            Console.WriteLine("# Date        {0:s}",
              DateTime.Now);
        }
    }
}
