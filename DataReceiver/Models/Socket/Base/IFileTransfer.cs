namespace DataReceiver.Models.Socket.Base
{
    public interface IFileTransfer
    {
        Task<bool> UploadAsync();
        Task<bool> DownloadAsync();
    }
}
