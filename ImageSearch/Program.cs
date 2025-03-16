using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Collections.Concurrent;

namespace ImageSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            String[] algorithms = new String[] { "exact", "euclidean" };

            if (args.Length != 4 ||
                !int.TryParse(args[2], out int threads) ||
                (threads <= 0 && threads != -1) ||
                !algorithms.Contains(args[3]) ||
                !CorrectImageFormat(args[0]) ||
                !CorrectImageFormat(args[1])
                )
            {
                Console.WriteLine("Usage: CPU-Process.exe <LargeImage.jpg | LargeImage.png | LargeImage.gif> <SmallImage.jpg | SmallImage.png | SmallImage.gif> <num_of_threads> <algorithm - exact | euclidean>");
                return;
            }

            try
            {
                Image largeImage = Image.FromFile(args[0]);
                Image smallImage = Image.FromFile(args[1]);

                Func<Color, Color, double> algorithm = args[3] == "exact"
                    ? (Func<Color, Color, double>)ColorExtensions.ExactDistance
                    : ColorExtensions.EuclideanDistance;

                List<int> ts;
                if (threads == -1)
                {
                    ts = new List<int> { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, Environment.ProcessorCount };
                    ts.Sort();
                }
                else
                {
                    ts = new List<int> { threads };
                }
                foreach (int t in ts)
                {
                    Console.WriteLine($"Running image searcher for {t} threads:");
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    ImageSearcher searcher = new ImageSearcher(largeImage, smallImage, t);
                    bool result = searcher.Search();

                    stopwatch.Stop();


                    if (result)
                    {
                        ConcurrentBag<(int x, int y)> matchCoordinates = searcher.GetMatchCoordinates();
                        foreach (var (x, y) in matchCoordinates)
                        {
                            Console.WriteLine($"{x},{y}");
                        }
                    }
                    Console.WriteLine("Time: " + stopwatch.Elapsed.TotalMilliseconds + "ms");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static bool CorrectImageFormat(string image)
        {
            return image.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                   image.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                   image.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                   image.EndsWith(".png", StringComparison.OrdinalIgnoreCase);
        }
    }
}
