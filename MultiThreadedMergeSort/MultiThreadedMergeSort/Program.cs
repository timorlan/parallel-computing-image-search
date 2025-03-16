using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        MTMergeSort sorter = new MTMergeSort();

        // Generate a large list of random strings
        Random random = new Random();
        List<string> largeList = new List<string>();
        for (int i = 0; i < 100000; i++) // This creates a list with 100,000 strings
        {
            largeList.Add(new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ", 10)
                .Select(s => s[random.Next(s.Length)]).ToArray()));
        }

        List<string> sortedList = sorter.MergeSort(largeList.ToArray());

        // Output first 10 sorted strings as a test
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine(sortedList[i]);
        }
    }
}
