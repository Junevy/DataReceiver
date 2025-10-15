namespace DataReceiver.Models.Socket.Base
{
    public interface IPlc
    {
        Task WriteRegisterAsync(int address, int value);
        Task<int> ReadRegisterAsync(int address);
    }
}
