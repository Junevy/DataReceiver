using System.Security.Claims;

namespace DataReceiver.Models.Socket.FTP
{
    public class FtpUser(string userName, string password, string? rootPath = null) : ClaimsPrincipal
    {
        public string UserName { get; set; } = userName;
        public string Password { get; set; } = password;
        public string RootPath { get; set; } = rootPath ?? userName;

    }
}
