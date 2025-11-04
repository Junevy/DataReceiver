using Common;
using RegularCleanupTask;
using RegularCleanupTask.TestConfigs;


//var testconfig1 = ConfigHelper.Build<TcpClientConfig>();


string targetPath = @"C:/Work";
int daysThreshold = 30;
int scanIntervalDays = 1;


var rootPath = Path.GetPathRoot(targetPath)?.TrimEnd('\\') ?? "";

if ("C:".Equals(rootPath))
{
    Console.WriteLine("The target path cannot be on the C drive. Exiting the application.");
    Helper.Log("The target path cannot be on the C drive. Exiting the application.");
    return;
}

Helper.GetInfo(targetPath, daysThreshold, scanIntervalDays);
var test = Core.GetLastDirectory(targetPath);
var singleDir = test.Split(',');

foreach (var dir in singleDir)
{
    if (!string.IsNullOrEmpty(dir) && dir != targetPath)
        Directory.Delete(dir, true);
}

Console.WriteLine("Cleanup task was completed, press any key to exit.");
Console.ReadKey();