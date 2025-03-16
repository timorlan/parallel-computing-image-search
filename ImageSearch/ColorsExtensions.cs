using System.Drawing;

public static class ColorExtensions
{
    public static double EuclideanDistance(this Color color, Color other)
    {
        double rDiff = color.R - other.R;
        double gDiff = color.G - other.G;
        double bDiff = color.B - other.B;
        double distance = Math.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
        return distance;
    }

    public static double ExactDistance(this Color color, Color other)
    {
        double distance = Math.Abs(color.R - other.R) + Math.Abs(color.G - other.G) + Math.Abs(color.B - other.B);
        return distance;
    }
}
