using Microsoft.Win32.TaskScheduler;

namespace Services.TaskSchedule
{
    public static class TaskScheduleService
    {

        public static void RegisterTask(string description, string exePath, short intervaldays, string taskName)
        {
            UnregisterTask(taskName); // 先删除已有任务，防止重复创建

            try
            {
                using TaskService ts = new();
                TaskDefinition td = ts.NewTask();

                td.Principal.UserId = "SYSTEM";
                td.Principal.LogonType = TaskLogonType.ServiceAccount;

                td.RegistrationInfo.Description = description;
                td.Actions.Add(new ExecAction(exePath, null)); // 立即执行一次，防止任务未创建前就错过触发时间
                td.Triggers.Add(new DailyTrigger()
                {
                    StartBoundary = DateTime.Today + TimeSpan.FromHours(2), // 每天凌晨2点执行
                    DaysInterval = intervaldays,
                });

                td.Settings.DisallowStartIfOnBatteries = false; // 允许在使用电池时运行
                td.Settings.StopIfGoingOnBatteries = false;     // 允许在使用电池时继续运行
                td.Settings.ExecutionTimeLimit = TimeSpan.FromMinutes(2); // 限制执行时长(设为0表示无限)

                ts.RootFolder.RegisterTaskDefinition(
                    taskName,
                    td,
                    TaskCreation.CreateOrUpdate,
                    null,
                    null,
                    td.Principal.LogonType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to register scheduled task: {ex.Message}");
            }

        }

        public static void UnregisterTask(string taskName)
        {
            using TaskService ts = new();
            try
            {
                ts.RootFolder.DeleteTask(taskName, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to unregister scheduled task: {ex.Message}");
            }
        }
    }
}
