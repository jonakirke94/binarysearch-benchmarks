using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binarysearch_benchmarks
{
    public class Program
    {

        public static int NumberOfTimesCalled = 0;
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            checkRecursive(1, 10000000, 45);
            NumberOfTimesCalled = 0;
            checkRecursive(1, 100000000,2500);
            NumberOfTimesCalled = 0;
            checkRecursive(1, 500000, 10000000);
            NumberOfTimesCalled = 0;
            checkRecursive(1, 4999, 25031903);

            Console.ReadKey();
        }
        
        public static void checkRecursive(int arrayFrom, int arrayTo, int key)
        {
            var startTime = Stopwatch.StartNew();
            int[] arr = createArray(arrayFrom, arrayTo).ToArray();
            recursiveBenchmark(arr, key, 0, arr.Length - 1);
            startTime.Stop();
            Console.WriteLine("number of milliseconds to run: " + startTime.Elapsed.Milliseconds.ToString());
            Console.WriteLine("Number of times run: " + NumberOfTimesCalled);
        }

        public static int[] createArray (int start, int end)
        {
            return Enumerable.Range(start, end).ToArray();
        }
        //pass min as 0 and max as array lenght -1.
        public static void recursiveBenchmark(int[] array, int NumberToSearchFor, int min, int max)
        {
            NumberOfTimesCalled++;
            if (min > max)
            {
                Console.WriteLine("min is bigger than max which is 0.");
            }
            else
            {
                int mid = (min + max) / 2;
                if(NumberToSearchFor == array[mid])
                {
                    Console.WriteLine(++mid);
                }else if(NumberToSearchFor < array[mid])
                {
                    recursiveBenchmark(array, NumberToSearchFor, min, mid-1);
                }
                else
                {
                    recursiveBenchmark(array, NumberToSearchFor, mid+1, max);
                }
            }  
        }
    }
}
