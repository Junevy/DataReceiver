using RegularCleanupTask;
using Services.Config;

// 确保配置为单例
string targetPath = ConfigService.GetNode("FtpServerConfig:RootPath");
int.TryParse(ConfigService.GetNode("FtpServerConfig:KeepDays"), out int daysThreshold);
int.TryParse(ConfigService.GetNode("FtpServerConfig:KeepDays"), out int checkIntervalHours);

// C 盘保护机制
var rootPath = Path.GetPathRoot(targetPath)?.TrimEnd('\\') ?? "";
if ("C:".Equals(rootPath) || string.IsNullOrEmpty(rootPath))
{
    Console.WriteLine("The target path cannot be on the C drive or null. Exiting the application.");
    Core.Log("The target path cannot be on the C drive. Exiting the application.");
    return;
}

// 输出配置信息
Core.GetInfo(targetPath, daysThreshold, checkIntervalHours);

// 删除扫描出符合规则的文件夹
var allDir = Core.GetLastDirectory(targetPath, daysThreshold);
var singleDir = allDir.Split(',');

// 遍历删除
foreach (var dir in singleDir)
{
    // 避免误删目标路径
    if (!string.IsNullOrEmpty(dir) && dir != targetPath)
    {
        try
        {
            Core.Log($"Deleting directory: {dir}");
            Directory.Delete(dir, true);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error deleting directory {dir}: {e.Message}");
            Core.Log($"Error deleting directory {dir}: {e.Message}");
        }
    }
}


Console.WriteLine("Cleanup task was completed, press any key to exit.");
Core.Log(Environment.NewLine);
Console.ReadKey();