using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haru.Hashing;
using Haru.IO;
using Haru.Shared;

namespace Haru.FileChecker
{
    public class ConsistencyChecker
    {
        private const int _bufferSize = 131072; // 128 KB

        public static ConsistencyMetadata GetMetadata(string basepath, string filepath, bool? isCritical = true)
        {
            // fix filepath (remove basepath, change separator to unix-style)
            var file = filepath
                .ReplaceFirst($"{basepath}\\", string.Empty)
                .Replace('\\', '/');

            var size = VFS.GetFileSize(filepath);
            var hash = string.Empty;
            int? checksum = null;

            // todo: is reusing filestream allowed?
            if (isCritical != null & isCritical == true)
            {
                using (var fs = VFS.GetFileReadStream(filepath))
                {
                    hash = Md5.Compute(fs, _bufferSize);
                }

                using (var fs = VFS.GetFileReadStream(filepath))
                {
                    checksum = EftChecksum.Compute(fs, _bufferSize);
                }
            }

            return new ConsistencyMetadata(file, size, hash, checksum, isCritical);
        }

        public static ConsistencyResult ValidateFile(string basepath, ConsistencyMetadata metadata)
        {
            // check file existance
            var filepath = VFS.CombinePath(basepath, metadata.Path);
            if (!VFS.Exists(filepath))
            {
                return ConsistencyResult.FileNotFound;
            }

            // generate metadata to compare to
            var compare = GetMetadata(basepath, filepath, metadata.IsCritical);

            // check file size
            if (compare.Size != metadata.Size)
            {
                return ConsistencyResult.FileSizeMismatch;
            }

            if (metadata.IsCritical != null && metadata.IsCritical == true)
            {
                // check file hash
                if (metadata.Hash != null && compare.Hash != metadata.Hash)
                {
                    return ConsistencyResult.FileHashMismatch;
                }

                // check file checksum
                if (metadata.Checksum != null && compare.Checksum != metadata.Checksum)
                {
                    return ConsistencyResult.FileChecksumMismatch;
                }
            }

            return ConsistencyResult.Success;
        }

        public static ConsistencyMetadata[] GetMetadatas(string basepath, List<string> files)
        {
            var metadatas = new ConcurrentBag<ConsistencyMetadata>();

            Parallel.ForEach(files, (filepath) =>
            {
                metadatas.Add(GetMetadata(basepath, filepath));
            });

            return metadatas.ToArray();
        }

        public static Dictionary<string, ConsistencyResult> ValidateFiles(
            string basepath,
            ConsistencyMetadata[] metadatas)
        {
            var results = new ConcurrentDictionary<string, ConsistencyResult>();

            Parallel.ForEach(metadatas, (metadata) =>
            {
                var result = ValidateFile(basepath, metadata);
                results.TryAdd(metadata.Path, result);
            });

            return new Dictionary<string, ConsistencyResult>(results);
        }
    }
}