namespace RegularCleanupTask
{
    public static class Core
    {
        private readonly static List<string> lastDirs = [];

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="targetPath"></param>
        /// <param name="daysThreshold"></param>
        /// <param name="scanIntervalDays"></param>
        public static void GetInfo(string targetPath, int daysThreshold, int scanIntervalDays)
        {
            Console.WriteLine("Auto cleanup task started.");
            Console.WriteLine("================================");
            Log("Auto cleanup task started.");
            Console.WriteLine($"Path: {targetPath}");
            Log($"Path: {targetPath}");
            Console.WriteLine($"Days: {daysThreshold} day(s)");
            Log($"Days: {daysThreshold} day(s)");
            Console.WriteLine($"Interval: {scanIntervalDays} hour(s)");
            Log($"Interval: {scanIntervalDays} hour(s)");
            Console.WriteLine("================================");
        }

        /// <summary>
        /// 日志功能
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Log(string message)
        {
            string currentLog = AppContext.BaseDirectory + DateTime.Now.ToString("yyyy-MM-dd") + "_cleanup.log";

            if (!File.Exists(currentLog))
                File.CreateText(currentLog).Dispose();

            File.AppendAllText(currentLog, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss:ff}]  {message}{Environment.NewLine}");
        }

        /// <summary>
        /// 手动清理任务
        /// </summary>
        /// <param name="targetPath">目标路径</param>
        /// <param name="daysThreshold">保留天数</param>
        /// <param name="scanIntervalDays">扫描间隔</param>
        public static void CleanupOldFiles(string targetPath, int daysThreshold, int scanIntervalDays)
        {
            int count = 0;

            if (!Directory.Exists(targetPath))
            {
                Console.WriteLine($"Path does not exist: {targetPath}");
                Core.Log($"Path does not exist: {targetPath}");
                return;
            }

            GetLastDirectory(targetPath, 1);

            try
            {
                var files = Directory.GetFileSystemEntries(targetPath);

                Console.Write($"\nFind {files.Length} files.");
                Core.Log($"Find {files.Length} files.");

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
                            Core.Log($"Deleted: {file}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting file {file}: {ex.Message}");
                        Core.Log($"Error deleting file {file}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Cleanup completed. and cleanup {count} files.");
                Core.Log($"Cleanup completed. and cleanup {count} files.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during cleanup: {ex.Message}");
                Core.Log($"Error during cleanup: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取文件最后一级目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="keepDays">保留天数</param>
        /// <returns></returns>
        public static string GetLastDirectory(string path, int keepDays)
        {
            var directories = Directory.GetDirectories(path);

            if (directories.Length == 0)
            {
                if (Directory.GetCreationTime(path) + TimeSpan.FromDays(keepDays) <= DateTime.Now)
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
