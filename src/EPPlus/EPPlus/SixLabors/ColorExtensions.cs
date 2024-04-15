using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Magicodes.IE.EPPlus.SixLabors
{
    internal static class ColorExtensions
    {
        public static string ToArgbHex(this Color color)
        {
            return $"{color.Alpha:X2}{color.Red:X2}{color.Green:X2}{color.Blue:X2}";
        }
    }
}