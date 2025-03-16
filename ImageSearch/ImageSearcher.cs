using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ImageSearch
{
    public class ImageSearcher
    {
        private readonly Image largeImage;
        private readonly Image smallImage;
        private readonly int numThreads;
        private readonly ConcurrentBag<(int x, int y)> matchCoordinates;

        public ImageSearcher(Image largeImage, Image smallImage, int numThreads)
        {
            this.largeImage = largeImage;
            this.smallImage = smallImage;
            this.numThreads = numThreads;
            this.matchCoordinates = new ConcurrentBag<(int x, int y)>();
        }

        public bool Search()
        {
            int width = largeImage.Width;
            int height = largeImage.Height;
            int sliceWidth = (width + numThreads - 1) / numThreads;

            using (CountdownEvent countdownEvent = new CountdownEvent(numThreads))
            {
                for (int i = 0; i < numThreads; i++)
                {
                    int startX = i * sliceWidth;
                    int endX = Math.Min(startX + sliceWidth + smallImage.Width - 1, width);

                    ThreadPool.QueueUserWorkItem(state =>
                    {
                        SearchSlice(startX, endX);
                        countdownEvent.Signal();
                    });
                }

                countdownEvent.Wait();
            }

            return matchCoordinates.Count > 0;
        }

        private void SearchSlice(int startX, int endX)
        {
            int smallWidth = smallImage.Width;
            int smallHeight = smallImage.Height;

            for (int x = startX; x < endX - smallWidth + 1; x++)
            {
                for (int y = 0; y < largeImage.Height - smallHeight + 1; y++)
                {
                    if (largeImage.CompareCroppedArea(x, y, smallImage, ColorExtensions.ExactDistance))
                    {
                        matchCoordinates.Add((x, y));
                    }
                }
            }
        }

        public ConcurrentBag<(int x, int y)> GetMatchCoordinates()
        {
            return matchCoordinates;
        }
    }
}
