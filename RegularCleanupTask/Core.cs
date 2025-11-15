namespace RegularCleanupTask
{
    public static class Core
    {
        private readonly static List<string> lastDirs = [];

        public static void CleanupOldFiles(string targetPath, int daysThreshold, int scanIntervalDays)
        {
            int count = 0;

            if (!Directory.Exists(targetPath))
            {
                Console.WriteLine($"Path does not exist: {targetPath}");
                Helper.Log($"Path does not exist: {targetPath}");
                return;
            }

            GetLastDirectory(targetPath, 1);

            try
            {
                var files = Directory.GetFileSystemEntries(targetPath);

                Console.Write($"\nFind {files.Length} files.");
                Helper.Log($"Find {files.Length} files.");

                DateTime thresholdDate = DateTime.Now.AddDays(-daysThreshold);
                foreach (var file in files)
                {
                    try
                    {
                        DateTime lastModified = File.GetCreationTime(file);
                        if (lastModified < thresholdDate)
                        {
                            //Directory.Delete(file, true);
                            File.Delete(file);
                            count++;
                            Console.WriteLine($"Deleted: {file}");
                            Helper.Log($"Deleted: {file}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting file {file}: {ex.Message}");
                        Helper.Log($"Error deleting file {file}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Cleanup completed. and cleanup {count} files.");
                Helper.Log($"Cleanup completed. and cleanup {count} files.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during cleanup: {ex.Message}");
                Helper.Log($"Error during cleanup: {ex.Message}");
            }
        }

        public static string GetLastDirectory(string path, int keepDays)
        {
            var directories = Directory.GetDirectories(path);

            if (directories.Length == 0)
            {
                if (Directory.GetCreationTime(path) + TimeSpan.FromDays(keepDays) < DateTime.Now)
                lastDirs.Add(path);
                return path;
            }

            foreach (var dir in directories)
            {
                GetLastDirectory(dir, keepDays);
            }

            return String.Join(",", [.. lastDirs]);
        }
    }
}
