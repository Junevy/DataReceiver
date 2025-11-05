using Microsoft.Extensions.DependencyInjection;
using RegularCleanupTask;

// 确保配置为单例
var config = DataReceiver
            .App.Current.Services
            .GetRequiredService<DataReceiver.Models.Socket.Config.FtpServerConfig>();

string targetPath = config.RootPath;
int daysThreshold = config.KeepDays;
int scanIntervalDays = config.ScanIntervalDays;

// C 盘保护机制
var rootPath = Path.GetPathRoot(targetPath)?.TrimEnd('\\') ?? "";
if ("C:".Equals(rootPath))
{
    Console.WriteLine("The target path cannot be on the C drive. Exiting the application.");
    Helper.Log("The target path cannot be on the C drive. Exiting the application.");
    return;
}

Helper.GetInfo(targetPath, daysThreshold, scanIntervalDays);

// 删除扫描出符合规则的文件夹
var allDir = Core.GetLastDirectory(targetPath);
var singleDir = allDir.Split(',');
foreach (var dir in singleDir)
{
    // 避免误删目标路径
    if (!string.IsNullOrEmpty(dir) && dir != targetPath)
        Directory.Delete(dir, true);
}

Console.WriteLine("Cleanup task was completed, press any key to exit.");
Console.ReadKey();