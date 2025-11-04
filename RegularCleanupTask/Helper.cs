namespace RegularCleanupTask
{
    public static class Helper
    {
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

        public static void Log(string message)
        {
            string currentLog = AppContext.BaseDirectory + DateTime.Now.ToString("yyyy-MM-dd") + "_cleanup.log";

            if (!File.Exists(currentLog))
                File.CreateText(currentLog).Dispose();

            File.AppendAllText(currentLog, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss:ff}]  {message}{Environment.NewLine}");
        }
    }
}