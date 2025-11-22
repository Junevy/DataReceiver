using DataReceiver.Models;
using DataReceiver.Models.Socket.Config;
using DataReceiver.Models.Socket.Interface;
using log4net;
using Microsoft.Win32.TaskScheduler;

namespace DataReceiver.Services.Decorator
{
    internal class ScheduleCleanDecorator(IConnection inner, TaskScheduleConfig config)
        : ConnectionDecoratorBase(inner), ITaskSchedulerCapable
    {
        protected static readonly new ILog Log = LogManager.GetLogger(typeof(ScheduleCleanDecorator));
        public TaskScheduleConfig Config { get; set; } = config;

        public void RegisterTask()
        {
            UnregisterTask(); // 先删除已有任务，防止重复创建

            try
            {
                using TaskService ts = new();
                TaskDefinition td = ts.NewTask();

                td.RegistrationInfo.Description = Config.Description;
                td.Actions.Add(new ExecAction(Config.ExePath, null)); // 立即执行一次，防止任务未创建前就错过触发时间
                td.Triggers.Add(new DailyTrigger()
                {
                    StartBoundary = DateTime.Today + TimeSpan.FromHours(2), // 每天凌晨2点执行
                    DaysInterval = Config.IntervalDays,
                });

                td.Settings.DisallowStartIfOnBatteries = false; // 允许在使用电池时运行
                td.Settings.StopIfGoingOnBatteries = false;     // 允许在使用电池时继续运行
                td.Settings.ExecutionTimeLimit = TimeSpan.FromMinutes(2); // 限制执行时长(设为0表示无限)

                ts.RootFolder.RegisterTaskDefinition(
                    Config.TaskName,
                    td,
                    TaskCreation.CreateOrUpdate,
                    null,
                    null,
                    TaskLogonType.ServiceAccount);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to register scheduled task: {ex.Message}");
            }

        }

        public void UnregisterTask()
        {
            using TaskService ts = new();
            try
            {
                ts.RootFolder.DeleteTask(Config.TaskName, false);
            }
            catch (Exception ex)
            {
                Log.Error($"Failed to unregister scheduled task: {ex.Message}");
            }
        }
    }
}
