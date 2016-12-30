using System;
using System.Windows.Media;

namespace Play.Flies.Client.Helper
{
    public static class ColorExtensions
    {
        public static Color MixRandomColor(this Color @base, int? seed = null)
        {
            var color = GetRandomColor(seed);

            var result = Color.FromRgb(
                (byte) ((color.R + @base.R) / 2),
                (byte) ((color.G + @base.G) / 2),
                (byte) ((color.B + @base.B) / 2)
            );

            return result;
        }

        public static Color GetRandomColor(int? seed = null)
        {
            var random = new Random(seed ?? Guid.NewGuid().GetHashCode());
            var red = (byte) random.Next(256);
            var green = (byte) random.Next(256);
            var blue = (byte) random.Next(256);

            var color = Color.FromRgb(red, green, blue);
            return color;
        }
    }
}