using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slicer
{
    public class FileSlicer
    {
        List<string> _chunkList = new List<string>();

        public void Slice(string filePath)
        {
            IEnumerable<string> chunks;
            var fileInfo = new FileInfo(filePath);
            using (var fileStream = ReadFile(filePath, out fileInfo))
            {
                chunks = CreateSlices(Convert.ToInt32(fileInfo.Length), fileStream, 512 * 1024, Guid.NewGuid());
            }
        }

        private IEnumerable<string> CreateSlices(int numBytes, FileStream fs, long chunkSize, Guid batchCode)
        {
            var numChunks = calculateNumberOfSlices(numBytes, chunkSize);

            for (var i = 0; i < numChunks; i++)
            {
                var sourceOffset = i * chunkSize;
                var chunkBufferSize = i != numChunks - 1 ? chunkSize : numBytes - sourceOffset;
                var chunk = new byte[chunkBufferSize];
            

                fs.Read(chunk, 0, Convert.ToInt32(chunkBufferSize));

                var chunkDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\SliceFolder\";
                if (!Directory.Exists(chunkDirectory))
                {
                    Directory.CreateDirectory(chunkDirectory);
                }

                var chunkFilePath = String.Format("{0}{1}-{2}-{3}.enc", chunkDirectory, batchCode, i);

                var readBufferLength = Convert.ToInt32(chunkSize);

                if (i >= numChunks - 1)
                {
                    readBufferLength = Convert.ToInt32(fs.Length - (i * chunkSize));
                }
                using (var writeStream = new FileStream(chunkFilePath, FileMode.CreateNew))
                {
                    writeStream.Write(chunk, 0, readBufferLength);
                }
                _chunkList.Add(chunkFilePath);
            }

            return _chunkList;
        }

        private static long calculateNumberOfSlices(long documentLength, long chunkSize)
        {
            if (chunkSize <= 0) { return 0; }

            var chunkCount = (documentLength / chunkSize);

            var hasRemainder = (documentLength % chunkSize) > 0;
            if (hasRemainder) { chunkCount++; }

            return chunkCount;
        }

        private FileStream ReadFile(string fileName, out FileInfo fileInfo)
        {
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            fileInfo = new FileInfo(fileName);
            if ((int)fileInfo.Length == 0)
            {
                throw new FileLoadException("File was zero bytes");
            }
            return fileStream;
        }

        public void Reassemble()
        {
            using (var writeStream = new FileStream(@"..\..\..\files\reassembled_" + Guid.NewGuid() + ".txt", FileMode.CreateNew, FileAccess.Write))
            {
                foreach (var chunk in _chunkList)
                {
                    using (var chunkStream = new FileStream(chunk, FileMode.Open, FileAccess.Read))
                    {
                        var chunkBuffer = new byte[chunkStream.Length];
                        chunkStream.Read(chunkBuffer, 0, Convert.ToInt32(chunkStream.Length));
                        writeStream.Write(chunkBuffer, 0, Convert.ToInt32(chunkStream.Length));
                    }
                    File.Delete(chunk);
                }
            }
        }
    }
}
