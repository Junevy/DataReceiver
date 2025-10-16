namespace DataReceiver.Models.Socket.Interface
{
    public interface IPlc
    {
        Task WriteRegisterAsync(int address, int value);
        Task<int> ReadRegisterAsync(int address);
    }
}
