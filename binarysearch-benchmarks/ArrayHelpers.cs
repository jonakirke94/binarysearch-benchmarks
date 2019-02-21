using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binarysearch_benchmarks
{
    public static class ArrayHelpers
    {
        public static void Swap(int[] arr, int s, int t)
        {
            int tmp = arr[s]; arr[s] = arr[t]; arr[t] = tmp;
        }

        public static void Shuffle(int[] arr)
        {
            for (int i = arr.Length - 1; i > 0; i--)
                Swap(arr, i, new Random().Next(i + 1));
        }

        public static int[] FillIntArray(int n)
        {
            int[] arr = new int[n];
            for (int i = 0; i < n; i++)
                arr[i] = i;
            return arr;
        }
    }
}
