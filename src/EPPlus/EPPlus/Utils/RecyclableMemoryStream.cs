using Microsoft.IO;
using System.IO;

namespace OfficeOpenXml.Utils
{
    public static class RecyclableMemoryStream
    {
        private static readonly RecyclableMemoryStreamManager _memoryManager = new RecyclableMemoryStreamManager();

        static RecyclableMemoryStream()
        {
            _memoryManager = new RecyclableMemoryStreamManager(new RecyclableMemoryStreamManager.Options()
            {
                //MaximumSmallPoolFreeBytes = 64 * 1024 * 32,
                //MaximumLargePoolFreeBytes = 64 * 1024 * 1024,
                //AggressiveBufferReturn = true
            });
        }

        private const string TagSource = "Magicodes.EPPlus";

        internal static MemoryStream GetStream()
        {
            return _memoryManager.GetStream(TagSource);
        }

        internal static MemoryStream GetStream(byte[] array)
        {
            return _memoryManager.GetStream(array);
        }

        internal static MemoryStream GetStream(int capacity)
        {
            return _memoryManager.GetStream(TagSource, capacity);
        }
    }
}