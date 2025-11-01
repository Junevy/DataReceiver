string targetPath = @"D:/Work";
int daysThreshold = 30;
int scanIntervalDays = 1;

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

Console.WriteLine("\nPress 'S' start the App. Press ANY KEY to exit the App.\n");
var key = Console.ReadKey();

if (key.Key != ConsoleKey.S)
    return;

Console.WriteLine("\nAre you sure to start the cleanup task? Press 'Y' to confirm, or ANY KEY to exit.\n");
if (Console.ReadKey().Key != ConsoleKey.Y)
    return;


CleanupOldFiles(targetPath, daysThreshold, scanIntervalDays);

Console.WriteLine("Cleanup task was completed, press any key to exit.");
Console.ReadKey();


static void CleanupOldFiles(string targetPath, int daysThreshold, int scanIntervalDays)
{
    int count = 0;

    if (!Directory.Exists(targetPath))
    {
        Console.WriteLine($"Path does not exist: {targetPath}");
        Log($"Path does not exist: {targetPath}");
        return;
    }

    try
    {
        var files = Directory.GetFileSystemEntries(targetPath);

        Console.Write($"\nFind {files.Length} files.");
        Log($"Find {files.Length} files.");

        DateTime thresholdDate = DateTime.Now.AddDays(-daysThreshold);
        foreach (var file in files)
        {
            try
            {
                DateTime lastModified = File.GetCreationTime(file);
                if (lastModified < thresholdDate)
                {
                    File.Delete(file);
                    count++;
                    Console.WriteLine($"Deleted: {file}");
                    Log($"Deleted: {file}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file {file}: {ex.Message}");
                Log($"Error deleting file {file}: {ex.Message}");
            }
        }

        Console.WriteLine($"Cleanup completed. and cleanup {count} files.");
        Log($"Cleanup completed. and cleanup {count} files.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during cleanup: {ex.Message}");
        Log($"Error during cleanup: {ex.Message}");
    }
}

static void Log(string message)
{
    string currentLog = AppContext.BaseDirectory + DateTime.Now.ToString("yyyy-MM-dd") + "_cleanup.log";

    if (!File.Exists(currentLog))
        File.CreateText(currentLog).Dispose();

    File.AppendAllText(currentLog, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss:ff}]  {message}{Environment.NewLine}");
}