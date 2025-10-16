namespace DataReceiver.Models.Socket.Interface
{
    public interface IFileTransfer
    {
        Task<bool> UploadAsync();
        Task<bool> DownloadAsync();
    }
}
