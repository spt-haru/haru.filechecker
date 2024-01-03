using System;
using Newtonsoft.Json;
using Haru.IO;
using Haru.FileChecker;

namespace Haru.FileChecker.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Haru.FilesChecker";

            if (args.Length != 3)
            {
                return;
            }

            var mode = args[0].Split("--mode=")[1];
            var basepath = args[1].Split("--path=")[1];
            var configfile = args[2].Split("--config=")[1];

            switch (mode)
            {
                case "generate":
                    RunGenerateMode(basepath, configfile);
                    break;

                case "validate":
                    RunValidateMode(basepath, configfile);
                    break;
            }
        }

        private static void RunGenerateMode(string basepath, string configfile)
        {
            // get paths
            var files = VFS.GetFilesRecursive(basepath);

            // get consistencyinfo
            Console.WriteLine($"Found {files.Count} files.");
            Console.WriteLine($"Processing...");
            var metadatas = ConsistencyChecker.GetMetadatas(basepath, files);
            var config = new ConsistencyInfo(metadatas);

            // save file
            Console.WriteLine($"Generating ConsistencyInfo...");
            var text = JsonConvert.SerializeObject(config);
            VFS.WriteText(configfile, text);

            Console.WriteLine("Done.");
        }

        private static void RunValidateMode(string basepath, string configfile)
        {
            // get entries
            var text = VFS.ReadText(configfile);
            var config = JsonConvert.DeserializeObject<ConsistencyInfo>(text);

            // get file statuses
            Console.WriteLine($"Validating...");
            var results = ConsistencyChecker.ValidateFiles(basepath, config.Entries);

            // pritn results
            foreach (var file in results.Keys)
            {
                var result = results[file];

                switch (result)
                {
                    case ConsistencyResult.FileNotFound:
                        Console.WriteLine($"{file} doesn't exist.");
                        break;

                    case ConsistencyResult.FileSizeMismatch:
                        Console.WriteLine($"{file} size mismatch.");
                        break;

                    case ConsistencyResult.FileHashMismatch:
                        Console.WriteLine($"{file} hash mismatch.");
                        break;

                    case ConsistencyResult.FileChecksumMismatch:
                        Console.WriteLine($"{file} checksum mismatch.");
                        break;

                    case ConsistencyResult.Success:
                    default:
                        break;
                }
            }

            Console.WriteLine("Done.");
        }
    }
}