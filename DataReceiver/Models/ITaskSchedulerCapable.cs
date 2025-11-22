using DataReceiver.Models.Socket.Config;

namespace DataReceiver.Models
{
    public interface ITaskSchedulerCapable
    {
        TaskScheduleConfig Config { get; }

        void RegisterTask();
        void UnregisterTask();
    }
}
