using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ImageSearch
{
    public class Image
    {
        private Color[,] colors;
        public int Width { get; }
        public int Height { get; }

        public Image(int width, int height)
        {
            Width = width;
            Height = height;
            colors = new Color[width, height];
        }

        public void SetColor(int x, int y, Color color)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException("Coordinates are out of bounds.");
            colors[x, y] = color;
        }

        public static Image FromFile(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath), "File path is null");
            if (!File.Exists(filePath))
                throw new ArgumentException("File does not exist", nameof(filePath));
            if (new FileInfo(filePath).Length == 0)
                throw new ArgumentException("File is empty", nameof(filePath));

            using (Bitmap bitmap = new Bitmap(filePath))
            {
                int width = bitmap.Width;
                int height = bitmap.Height;
                Image image = new Image(width, height);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Color pixelColor = bitmap.GetPixel(x, y);
                        image.SetColor(x, y, pixelColor);
                    }
                }
                return image;
            }
        }

        public bool Contains(Image other, Func<Color, Color, double> distanceAlgorithm)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other), "Other image is null");
            if (distanceAlgorithm == null)
                throw new ArgumentNullException(nameof(distanceAlgorithm), "Distance algorithm is null");

            int height = colors.GetLength(1);
            int width = colors.GetLength(0);
            int otherHeight = other.colors.GetLength(1);
            int otherWidth = other.colors.GetLength(0);

            if (height < otherHeight || width < otherWidth)
            {
                return false;
            }

            for (int i = 0; i <= height - otherHeight; i++)
            {
                for (int j = 0; j <= width - otherWidth; j++)
                {
                    if (IsMatch(j, i, other, distanceAlgorithm))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsMatch(int startX, int startY, Image other, Func<Color, Color, double> distanceAlgorithm)
        {
            int otherWidth = other.colors.GetLength(0);
            int otherHeight = other.colors.GetLength(1);

            for (int i = 0; i < otherWidth; i++)
            {
                for (int j = 0; j < otherHeight; j++)
                {
                    Color largeColor = Color.FromArgb(colors[startX + i, startY + j].R, colors[startX + i, startY + j].G, colors[startX + i, startY + j].B);
                    Color smallColor = Color.FromArgb(other.colors[i, j].R, other.colors[i, j].G, other.colors[i, j].B);

                    double distance = distanceAlgorithm(largeColor, smallColor);
                    if (distance != 0)
                    {
                        Console.WriteLine($"Mismatch at large image ({startX + i}, {startY + j}) vs small image ({i}, {j}): distance {distance}");
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CompareCroppedArea(int startX, int startY, Image croppedImage, Func<Color, Color, double> distanceAlgorithm)
        {
            int mismatchCount = 0;

            for (int i = 0; i < croppedImage.Width; i++)
            {
                for (int j = 0; j < croppedImage.Height; j++)
                {
                    Color largeColor = Color.FromArgb(colors[startX + i, startY + j].R, colors[startX + i, startY + j].G, colors[startX + i, startY + j].B);
                    Color smallColor = Color.FromArgb(croppedImage.colors[i, j].R, croppedImage.colors[i, j].G, croppedImage.colors[i, j].B);

                    double distance = distanceAlgorithm(largeColor, smallColor);
                    if (distance != 0)
                    {
                        mismatchCount++;
                    }
                }
            }

            return mismatchCount == 0;
        }
    }
}
