using OfficeOpenXml.Utils;
using System;
using System.IO;

namespace Magicodes.IE.EPPlus
{
    public class ExcelImage : IDisposable
    {
        private byte[] _content;
        private ExcelImageFormat _format = ExcelImageFormat.Jpeg;
        private string _contentType;

        public ExcelImage(byte[] content, MemoryStream stream)
        {
            double width = 0, height = 0;
            _content = content;
            _format = ImageReader.GetPictureType(stream) ?? ExcelImageFormat.Jpeg;
            _contentType = MimeType(_format);

            ImageReader.TryGetImageBounds(_format, stream, ref width, ref height, out var horizontalResolution, out var verticalResolution);
            Width = (int)width;
            Height = (int)height;
            HorizontalResolution = (float)horizontalResolution;
            VerticalResolution = (float)verticalResolution;
        }

        public int Width { get; private set; }

        public int Height { get; private set; }
        public float HorizontalResolution { get; private set; }

        public float VerticalResolution { get; private set; }

        public ExcelImageFormat Format => _format;
        public string ContentType => _contentType;

        public string FileExtension => GetFileExtension(_format);

        public static ExcelImage Decode(byte[] buffer)
        {
            using var memoryStream = RecyclableMemoryStream.GetStream(buffer);
            return new(buffer, memoryStream);
        }

        public static ExcelImage Decode(Stream stream)
        {
            if (stream is MemoryStream memory)
            {
                return new ExcelImage(memory.ToArray(), memory);
            }
            using var memoryStream = RecyclableMemoryStream.GetStream();
            stream.CopyTo(memoryStream);
            return new ExcelImage(memoryStream.ToArray(), memoryStream);
        }

        public static ExcelImage Decode(string path)
        {
            return Decode(File.ReadAllBytes(path));
        }

        public bool Encode(Stream stream)
        {
            stream.Write(_content);
            return true;
        }

        public byte[] GetContent() => _content;

        public static IImageFormat DetectFormat(byte[] header)
        {
            using var memoryStream = RecyclableMemoryStream.GetStream(header);
            return ImageReader.GetPictureType(memoryStream) ?? ExcelImageFormat.Jpeg;
        }

        public static string MimeType(ExcelImageFormat source)
        {
            return source switch
            {
                ExcelImageFormat.Jpeg => "image/jpeg",
                ExcelImageFormat.Png => "image/png",
                ExcelImageFormat.Webp => "image/webp",
                ExcelImageFormat.Ico => "image/x-icon",
                ExcelImageFormat.Svg => " image/svg+xml",
                ExcelImageFormat.Tif => "image/tiff",
                ExcelImageFormat.Bmp => "image/bmp",
                ExcelImageFormat.Gif => "image/gif",
                ExcelImageFormat.Emf => "image/x-emf",
                ExcelImageFormat.Wmf => "application/x-msmetafile",
                ExcelImageFormat.Emz => "application/x-msmetafile",
                ExcelImageFormat.Wmz => "application/x-msmetafile",
                _ => "image/jpeg",
            };
        }

        public static string GetFileExtension(ExcelImageFormat source)
        {
            return source switch
            {
                ExcelImageFormat.Jpeg => "jpeg ",
                ExcelImageFormat.Png => "png",
                ExcelImageFormat.Webp => "webp",
                ExcelImageFormat.Ico => "icon",
                ExcelImageFormat.Svg => "svg",
                ExcelImageFormat.Tif => "tiff",
                ExcelImageFormat.Bmp => "bmp",
                ExcelImageFormat.Gif => "gif",
                ExcelImageFormat.Emf => "emf",
                ExcelImageFormat.Wmf => "wmf",
                ExcelImageFormat.Emz => "emz",
                ExcelImageFormat.Wmz => "wmz",
                _ => "jpeg",
            };
        }

        public void Dispose()
        {
            _content = null;
            GC.SuppressFinalize(this);
        }
    }
}

namespace SixLabors.ImageSharp { internal class NullClass { } }

namespace SixLabors.ImageSharp.PixelFormats { internal class NullClass { } }

namespace SixLabors.ImageSharp.Formats { internal class NullClass { } }


namespace SixLabors.ImageSharp.Formats.Jpeg { internal class NullClass { } }