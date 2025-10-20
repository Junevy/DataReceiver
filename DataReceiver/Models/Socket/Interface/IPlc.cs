namespace DataReceiver.Models.Socket.Interface
{
    /// <summary>
    /// Plc Socket的接口
    /// </summary>
    public interface IPlc
    {
        Task WriteRegisterAsync(int address, int value);
        Task<int> ReadRegisterAsync(int address);
    }
}
