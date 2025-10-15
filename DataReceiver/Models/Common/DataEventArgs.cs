namespace DataReceiver.Models.CommunicationCommon
{
    /// <summary>
    /// 新数据事件参数
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="data">接收到的数据</param>
    /// <param name="receivedTime">接收到数据的时间</param>
    /// <param name="length">接收消息的长度</param>
    /// <param name="message">携带的信息</param>
    public class DataEventArgs<T>
        (ReadOnlyMemory<T> data, int length, DateTime? receivedTime = null, string message = "") : EventArgs
    {
        public ReadOnlyMemory<T> Data { get; set;} = data;
        public DateTime ReceivedTime { get; set; } = receivedTime ?? DateTime.Now;
        public string Message { get; set; } = message;
        public int Length { get; set; } = length;
    }
}
